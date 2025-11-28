import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, TextInput, ScrollView, Modal, Alert, Dimensions, Linking, Clipboard } from 'react-native';
import { showMessage } from "react-native-flash-message";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';

import api from '../../../../services/api';
import SERVER_URL from '../../../../services/url';

const { width, height } = Dimensions.get('window');

export default function Planejamento({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [errorMessage, setErrorMessage] = useState(null);
  const [filtroAtivo, setFiltroAtivo] = useState('pendente');
  const [termoBusca, setTermoBusca] = useState('');
  const [usuarioState, setusuarioState] = useState(usuario);
  const [modalProgressoVisible, setModalProgressoVisible] = useState(false);
  const [modalPdfVisible, setModalPdfVisible] = useState(false);
  const [tarefaSelecionada, setTarefaSelecionada] = useState(null);
  const [progresso, setProgresso] = useState({});
  const [paginaAtual, setPaginaAtual] = useState(1);
  const [carregandoPdf, setCarregandoPdf] = useState(false);

  useFocusEffect(
    React.useCallback(() => {
      listarDados();
    }, [])
  );

  useEffect(() => {
    if (route.params?.usuario) {
      setusuarioState(route.params.usuario);
    }
  }, [route.params?.usuario]);

  async function obterInfoTarefa(tarefa) {
    try {
      const res = await api.post('dev4tech/obter_info_tarefa.php', {
        id_tarefa: tarefa.id_tarefa
      });
      
      if (res.data.success) {
        console.log("Informações da tarefa obtidas:", res.data);
        return {
          ...tarefa,
          total_paginas: res.data.total_paginas || 0,
          processada: res.data.total_paginas > 0
        };
      } else {
        console.log("Erro ao obter info da tarefa:", res.data.message);
        return {
          ...tarefa,
          total_paginas: 0,
          processada: false
        };
      }
    } catch (error) {
      console.log("Erro ao obter info da tarefa:", error);
      return {
        ...tarefa,
        total_paginas: 0,
        processada: false
      };
    }
  }
  async function dividirPDF(tarefa) {
    try {
      setCarregandoPdf(true);

      const nomePdf = tarefa.nome_arquivo.toLowerCase().endsWith('.pdf')
        ? tarefa.nome_arquivo
        : `${tarefa.nome_arquivo}.pdf`;

      const caminhoArquivo = `${SERVER_URL}dev4tech/arquivos/${nomePdf}`;
      console.log("Iniciando divisão do PDF:", nomePdf);
      console.log("Caminho do arquivo:", caminhoArquivo);

      // Primeiro: dividir o PDF fisicamente
      const resDivisao = await api.post('dev4tech/dividir_pdf.php', {
        caminho_arquivo: 'C:/xampp/htdocs/dev4tech/arquivos/' + nomePdf,
        id_tarefa: tarefa.id_tarefa
      });

      console.log("Resposta completa da divisão:", resDivisao);
      console.log("Dados da resposta:", resDivisao.data);

      if (!resDivisao.data.success) {
        console.log("ERRO NA DIVISÃO DO PDF:", resDivisao.data.message);
        showMessage({
          message: 'Erro',
          description: 'Falha ao dividir o PDF: ' + resDivisao.data.message,
          type: "danger",
          duration: 5000,
        });
        return;
      }

      // Verificar o modo de divisão
      if (resDivisao.data.modo === 'simulacao') {
        console.log("PDF dividido em MODO SIMULAÇÃO - todas as páginas são cópias do original");
        showMessage({
          message: 'Aviso',
          description: `PDF processado em modo simulação (${resDivisao.data.total_paginas} páginas)`,
          type: "warning",
          duration: 4000,
        });
      } else {
        console.log("PDF dividido com SUCESSO - páginas individuais criadas");
        showMessage({
          message: 'Sucesso',
          description: `PDF dividido em ${resDivisao.data.total_paginas} páginas individuais`,
          type: "success",
          duration: 4000,
        });
      }

      // Segundo: salvar no banco de dados
      console.log("Salvando no banco...");
      const resProcessamento = await api.post('dev4tech/processar_pdf.php', {
        id_tarefa: tarefa.id_tarefa,
        nome_arquivo: tarefa.nome_arquivo,
        total_paginas: resDivisao.data.total_paginas,
        hash_arquivo: tarefa.nome_arquivo + '_' + Date.now(),
      });

      console.log("Resposta do processamento:", resProcessamento.data);

      if (!resProcessamento.data.success) {
        showMessage({
          message: 'Erro no Banco',
          description: 'PDF dividido mas erro ao salvar no banco: ' + resProcessamento.data.message,
          type: "danger",
          duration: 5000,
        });
        return;
      }

      // Recarregar os dados para atualizar a lista
      listarDados();
      
    } catch (error) {
      console.log("ERRO COMPLETO NA DIVISÃO DO PDF:", error.message);
      console.log("Stack trace:", error.stack);
      showMessage({
        message: 'Erro',
        description: 'Falha completa ao processar o PDF: ' + error.message,
        type: "danger",
        duration: 6000,
      });
    } finally {
      setCarregandoPdf(false);
    }
  }

  // Função principal ao clicar em uma tarefa
  async function handleTarefaClick(tarefa) {
    // Primeiro, obter informações atualizadas da tarefa
    const tarefaAtualizada = await obterInfoTarefa(tarefa);
    
    // Se já foi processado antes (tem total_paginas), mostrar modal com progresso
    if (tarefaAtualizada.processada && tarefaAtualizada.total_paginas > 0) {
      await carregarProgresso(tarefaAtualizada);
      setTarefaSelecionada(tarefaAtualizada);
      setModalProgressoVisible(true);
      return;
    }

    // Se não foi processado, dividir o PDF
    await dividirPDF(tarefaAtualizada);
  }

  // Carregar progresso da leitura
  async function carregarProgresso(tarefa) {
    try {
      const res = await api.post('dev4tech/obter_progresso.php', {
        id_tarefa: tarefa.id_tarefa,
        id_funcionario: usuario.FuncionarioId
      });

      if (res.data.success) {
        setProgresso(res.data.progresso);
      }
    } catch (error) {
      console.log("Erro ao carregar progresso:", error);
    }
  }

  // Função quando uma página é visualizada
  async function marcarPaginaVisualizada(numeroPagina) {
    if (!tarefaSelecionada) return;

    try {
      const res = await api.post('dev4tech/marcar_pagina_visualizada.php', {
        id_tarefa: tarefaSelecionada.id_tarefa,
        id_funcionario: usuario.FuncionarioId,
        numero_pagina: numeroPagina
      });

      if (res.data.success) {
        await carregarProgresso(tarefaSelecionada);
        
        // Se concluiu todas as páginas, mover para concluído
        if (res.data.progresso.concluida) {
          await atualizarStatusTarefa(tarefaSelecionada.id_tarefa, 'concluido');
          listarDados();
          setModalPdfVisible(false);
          setModalProgressoVisible(false);
        } else if (res.data.progresso.total_paginas_visualizadas > 0) {
          // Se começou a ler, mover para fazendo
          await atualizarStatusTarefa(tarefaSelecionada.id_tarefa, 'fazendo');
          listarDados();
        }
      }
    } catch (error) {
      console.log("Erro ao marcar página:", error);
    }
  }

  // Atualizar status da tarefa
  async function atualizarStatusTarefa(idTarefa, status) {
    try {
      await api.post('dev4tech/atualizar_status_tarefa.php', {
        id_tarefa: idTarefa,
        status: status
      });
    } catch (error) {
      console.log("Erro ao atualizar status:", error);
    }
  }

  // Abrir modal para visualizar uma página específica
  async function visualizarPagina(numeroPagina) {
    setPaginaAtual(numeroPagina);
    setModalProgressoVisible(false);
    setModalPdfVisible(true);
    
    // Marcar a página como visualizada após um pequeno delay
    setTimeout(() => {
      marcarPaginaVisualizada(numeroPagina);
    }, 1000);
  }


  // Função para fazer download do PDF - MELHORADA
  async function fazerDownloadPagina(numeroPagina) {
    if (!tarefaSelecionada) return;

    try {
      const nomePagina = `tarefa_${tarefaSelecionada.id_tarefa}_pagina_${numeroPagina}`;
      const nomePaginaCorrigido = nomePagina.toLowerCase().endsWith(".pdf")
        ? nomePagina
        : nomePagina + ".pdf";
      const url = `${SERVER_URL}dev4tech/arquivos/${nomePaginaCorrigido}`;
      
      console.log("URL para download:", url);
      
      // Testar se a URL é acessível
      try {
        const response = await fetch(url, { method: 'HEAD' });
        if (!response.ok) {
          throw new Error('Arquivo não encontrado no servidor');
        }
      } catch (error) {
        console.log("Arquivo não acessível:", error);
        Alert.alert(
          'Arquivo Não Encontrado',
          `O arquivo da página ${numeroPagina} não foi encontrado no servidor.`,
          [{ text: 'OK' }]
        );
        return;
      }

      Alert.alert(
        'Download da Página',
        `O que você deseja fazer com a Página ${numeroPagina}?`,
        [
          { 
            text: 'Abrir no Navegador', 
            onPress: () => abrirNoNavegador(url) 
          },
          { 
            text: 'Copiar URL', 
            onPress: () => copiarParaAreaDeTransferencia(url) 
          },
          { 
            text: 'Cancelar', 
            style: 'cancel' 
          }
        ]
      );

    } catch (error) {
      console.log("Erro no download:", error);
      showMessage({
        message: 'Erro',
        description: 'Falha ao preparar download',
        type: "danger",
        duration: 3000,
      });
    }
  }

  // Função para copiar URL para a área de transferência
  async function copiarParaAreaDeTransferencia(texto) {
    try {
      await Clipboard.setString(texto);
      showMessage({
        message: 'URL copiada',
        description: 'Cole no navegador para fazer download',
        type: "success",
        duration: 3000,
      });
    } catch (error) {
      console.log("Erro ao copiar:", error);
      showMessage({
        message: 'Erro',
        description: 'Falha ao copiar URL',
        type: "danger",
        duration: 3000,
      });
    }
  }

  // Função para abrir no navegador
  async function abrirNoNavegador(url) {
    try {
      const canOpen = await Linking.canOpenURL(url);
      
      if (canOpen) {
        await Linking.openURL(url);
      } else {
        throw new Error('Não foi possível abrir esta URL');
      }
    } catch (error) {
      console.log("Erro ao abrir no navegador:", error);
      showMessage({
        message: 'Erro',
        description: 'Não foi possível abrir no navegador. Use a opção "Copiar URL".',
        type: "danger",
        duration: 4000,
      });
    }
  }

  async function listarDados() {
    if (!usuario?.FuncionarioId) {
      console.log("ID do usuário não disponível");
      return;
    }
    try {
      setErrorMessage(null);
      const res = await api.get(`dev4tech/tarefa.php`, {
        params: { id_funcionario: usuario.FuncionarioId }
      });

      if (res.data.success) {
        // Para cada tarefa, buscar informações completas
        const tarefasComInfo = await Promise.all(
          res.data.result.map(async (tarefa) => {
            const tarefaCompleta = await obterInfoTarefa(tarefa);
            
            // Lógica para determinar status baseado no progresso
            let status = 'pendente';
            
            // Se a tarefa foi processada e tem páginas, verificar progresso
            if (tarefaCompleta.processada) {
              try {
                const resProgresso = await api.post('dev4tech/obter_progresso.php', {
                  id_tarefa: tarefaCompleta.id_tarefa,
                  id_funcionario: usuario.FuncionarioId
                });
                
                if (resProgresso.data.success) {
                  const progresso = resProgresso.data.progresso;
                  if (progresso.concluida) {
                    status = 'concluido';
                  } else if (progresso.total_paginas_visualizadas > 0) {
                    status = 'fazendo';
                  }
                }
              } catch (error) {
                console.log("Erro ao verificar progresso:", error);
              }
            }

            return {
              ...tarefaCompleta,
              status_tarefa: status,
              pendente: status === 'pendente',
              fazendo: status === 'fazendo',
              concluido: status === 'concluido'
            };
          })
        );
        
        setDados(tarefasComInfo);
      } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
      }
    }
    catch (error) {
      console.log("Erro ao listar Arquivos:", error);
      setErrorMessage("Erro de conexão com o servidor");
    }
  }

  // Resto do código de filtros e renderização...
  const filtrarArquivos = () => {
    let arquivosFiltradas = dados;
    
    switch(filtroAtivo) {
      case 'pendente':
        arquivosFiltradas = arquivosFiltradas.filter(item => item.status_tarefa === 'pendente');
        break;
      case 'fazendo':
        arquivosFiltradas = arquivosFiltradas.filter(item => item.status_tarefa === 'fazendo');
        break;
      case 'concluido':
        arquivosFiltradas = arquivosFiltradas.filter(item => item.status_tarefa === 'concluido');
        break;
      default:
    }
    
    if (termoBusca) {
      const termo = termoBusca.toLowerCase();
      arquivosFiltradas = arquivosFiltradas.filter(item => 
        item.nome_arquivo.toLowerCase().includes(termo)
      );
    }
    
    return arquivosFiltradas;
  };

  const renderTarefas = () => {
    const arquivosFiltradas = filtrarArquivos();
    
    if (arquivosFiltradas.length === 0) {
      return (
        <Text style={[styles.texto, { textAlign: 'center', marginTop: 20, color: theme.text }]}>
          Nenhuma tarefa {filtroAtivo} encontrada
        </Text>
      );
    }

    return arquivosFiltradas.map((item, index) => (
      <TouchableOpacity
        key={`${item.id_tarefa}-${index}`} 
        style={styles.containertarefas}
        onPress={() => handleTarefaClick(item)}
        disabled={carregandoPdf}
      >
        <View style={styles.linhaTarefa}>
          <Image 
            source={require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textosTarefa}>
            <View style={styles.linhaTituloStatus}>
              <Text style={styles.nomeTarefa}>{item.nomeTarefa}</Text>

              {item.status_tarefa === 'concluido' && (
                <View style={[styles.containerfiltro, { backgroundColor: '#4CAF50' }]}>
                  <Text style={styles.textofiltro}>Concluído</Text>
                </View>
              )}
              {item.status_tarefa === 'fazendo' && (
                <View style={[styles.containerfiltro, { backgroundColor: '#ff8400ff' }]}>
                  <Text style={styles.textofiltro}>Fazendo</Text>
                </View>
              )}
              {item.status_tarefa === 'pendente' && (
                <View style={[styles.containerfiltro, { backgroundColor:'#FFC107' }]}>
                  <Text style={styles.textofiltro}>Pendente</Text>
                </View>
              )}
            </View>
            
            <Text style={styles.textolistatitulo}>{item.nome_arquivo}</Text>

            {item.total_paginas && (
              <Text style={styles.textolistacargo}>
                {item.total_paginas} páginas
              </Text>
            )}

            {carregandoPdf && (
              <Text style={styles.textolistacargo}>Processando PDF...</Text>
            )}

          </View>
        </View>

        <View style={styles.linhaInfo}>
          <Text style={[styles.textolistacargo, { backgroundColor: corDificuldade(item.dificuldade) }]}>
            {item.dificuldade}
          </Text>
          <Text style={styles.textolistadata}>Até {formatarData(item.data_entrega)}</Text>
        </View>
      </TouchableOpacity>
    ));
  };

  // Modal de Progresso (Lista de Páginas) - SIMPLIFICADO
  const ModalProgresso = () => (
    <Modal
      animationType="slide"
      transparent={true}
      visible={modalProgressoVisible}
      onRequestClose={() => setModalProgressoVisible(false)}
    >
      <View style={styles.modalContainer}>
        <View style={styles.modalContent}>
          <Text style={styles.modalTitle}>
            {tarefaSelecionada?.nome_arquivo}
          </Text>
          
          {/* Barra de Progresso */}
          <View style={styles.progressContainer}>
            <View style={styles.progressBar}>
              <View 
                style={[
                  styles.progressFill, 
                  { 
                    width: `${progresso.percentual_concluido || 0}%`,
                    backgroundColor: progresso.percentual_concluido === 100 ? '#4CAF50' : '#1A5CFF'
                  }
                ]} 
              />
            </View>
            <Text style={styles.progressText}>
              {progresso.total_paginas_visualizadas || 0} de {progresso.total_paginas || 0} páginas ({progresso.percentual_concluido || 0}%)
            </Text>
          </View>

          {/* Lista de Páginas com Ações */}
          <ScrollView style={styles.pagesList}>
            {tarefaSelecionada && Array.from({ length: tarefaSelecionada.total_paginas || 0 }, (_, i) => i + 1).map(pagina => (
              <View key={pagina} style={styles.pageItemWithActions}>
                <View style={styles.pageInfo}>
                  <Text style={styles.pageNumber}>Página {pagina}</Text>
                  {progresso.paginas_visualizadas?.includes(pagina) && (
                    <Text style={styles.pageStatus}>✓ Visualizada</Text>
                  )}
                </View>
                <View style={styles.pageActions}>
                  <TouchableOpacity
                    style={[styles.actionButton, styles.downloadAction]}
                    onPress={() => {
                      fazerDownloadPagina(pagina);
                      visualizarPagina(pagina);
                    }}
                  >
                    <Text style={styles.actionButtonText}>Download</Text>
                  </TouchableOpacity>
                </View>
              </View>
            ))}
          </ScrollView>

          <TouchableOpacity
            style={styles.closeButton}
            onPress={() => setModalProgressoVisible(false)}
          >
            <Text style={styles.closeButtonText}>Fechar</Text>
          </TouchableOpacity>
        </View>
      </View>
    </Modal>
  );

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View>
          <Text style={styles.titulo}>Planejamento</Text>

          <View style={styles.areabotao}>
            <TouchableOpacity
              style={[styles.botao, { backgroundColor: filtroAtivo === 'pendente' ? '#1A5CFF' : theme.inputBackground }]}
              onPress={() => setFiltroAtivo('pendente')}
            >
              <Text style={[styles.textobotao, { color: filtroAtivo === 'pendente' ? '#fff' : theme.text3 }]}>
                Pendente
              </Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={[styles.botao, { backgroundColor: filtroAtivo === 'fazendo' ? '#1A5CFF' : theme.inputBackground }]}
              onPress={() => setFiltroAtivo('fazendo')}
            >
              <Text style={[styles.textobotao, { color: filtroAtivo === 'fazendo' ? '#fff' : theme.text3 }]}>
                Fazendo
              </Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={[styles.botao, { backgroundColor: filtroAtivo === 'concluido' ? '#1A5CFF' : theme.inputBackground }]}
              onPress={() => setFiltroAtivo('concluido')}
            >
              <Text style={[styles.textobotao, { color: filtroAtivo === 'concluido' ? '#fff' : theme.text3 }]}>
                Concluídos
              </Text>
            </TouchableOpacity>
          </View>
        </View>
        {renderTarefas()}
      </ScrollView>
      
      <ModalProgresso />
    </View>
  );
}

// Funções auxiliares (mantenha as existentes)
function formatarData(data) {
  if (!data) return "";
  const partes = data.split("-");
  if (partes.length !== 3) return data;
  return `${partes[2]}/${partes[1]}/${partes[0]}`;
}

function corDificuldade(dificuldade) {
  switch((dificuldade || '').toLowerCase()) {
    case 'fácil': return '#4CAF50';
    case 'médio': return '#FFC107';
    case 'difícil': return '#F44336';
    default: return '#adadad';
  }
}