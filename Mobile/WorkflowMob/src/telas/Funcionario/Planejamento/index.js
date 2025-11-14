import React, { useState, useEffect } from 'react';
import { 
  Text, View, TouchableOpacity, Image, TextInput, ScrollView, 
  Modal, Alert, Dimensions 
} from 'react-native';
import { showMessage } from "react-native-flash-message";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';

import api from '../../../../services/api';

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
      
      console.log("Iniciando divisão do PDF:", tarefa.nome_arquivo);
      
      // Primeiro: dividir o PDF fisicamente
      const resDivisao = await api.post('dev4tech/dividir_pdf.php', {
        caminho_arquivo: 'C:/xampp/htdocs/dev4tech/arquivos/' + tarefa.nome_arquivo + ".pdf",
        id_tarefa: tarefa.id_tarefa
      });

      console.log("Resposta da divisão:", resDivisao.data);

      if (!resDivisao.data.success) {
        console.log("nome:", 'C:/xampp/htdocs/dev4tech/arquivos/' + tarefa.nome_arquivo + ".pdf");
        console.log("ERRO NA DIVISÃO DO PDF:", resDivisao.data.message);
        showMessage({
          message: 'Erro',
          description: 'Falha ao dividir o PDF: ' + resDivisao.data.message,
          type: "danger",
          duration: 3000,
        });
        return;
      }

      // Segundo: salvar no banco de dados
      console.log("Salvando no banco...");
      const resProcessamento = await api.post('dev4tech/processar_pdf.php', {
        id_tarefa: tarefa.id_tarefa,
        nome_arquivo: tarefa.nome_arquivo,
        total_paginas: resDivisao.data.total_paginas,
        hash_arquivo: tarefa.nome_arquivo + '_' + Date.now(), // Hash único
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

      showMessage({
        message: 'Sucesso',
        description: `PDF dividido em ${resDivisao.data.total_paginas} páginas`,
        type: "success",
        duration: 3000,
      });

      // Recarregar os dados para atualizar a lista
      listarDados();
      
    } catch (error) {
      console.log("ERRO NA DIVISÃO DO PDF:", error.message);
      console.log("Stack:", error.stack);
      showMessage({
        message: 'Erro',
        description: 'Falha completa ao processar o PDF: ' + error.message,
        type: "danger",
        duration: 5000,
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

  // Navegar entre páginas no modal de PDF
  function navegarPagina(direcao) {
    const novaPagina = paginaAtual + direcao;
    if (novaPagina >= 1 && novaPagina <= (tarefaSelecionada?.total_paginas || 0)) {
      setPaginaAtual(novaPagina);
      marcarPaginaVisualizada(novaPagina);
    }
  }

  // Obter URL da página atual
  function getUrlPaginaAtual() {
    if (!tarefaSelecionada) return '';
    // Usar o caminho correto da pasta arquivos
    return `http://localhost/dev4tech/arquivos/${tarefaSelecionada.id_tarefa}/pagina_${paginaAtual}.pdf`;
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

          {item.status_tarefa === 'concluido'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#4CAF50' }]}>
              <Text style={styles.textofiltro}>Concluído</Text>
            </View>: null
          }
          {item.status_tarefa === 'fazendo'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#FFA500' }]}>
              <Text style={styles.textofiltro}>Fazendo</Text>
            </View>: null
          }
          {item.status_tarefa === 'pendente'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#adadadff' }]}>
              <Text style={styles.textofiltro}>Pendente</Text>
            </View>: null
          }
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

  // Modal de Progresso (Lista de Páginas)
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

          {/* Lista de Páginas */}
          <ScrollView style={styles.pagesList}>
            {tarefaSelecionada && Array.from({ length: tarefaSelecionada.total_paginas || 0 }, (_, i) => i + 1).map(pagina => (
              <TouchableOpacity
                key={pagina}
                style={[
                  styles.pageItem,
                  progresso.paginas_visualizadas?.includes(pagina) && styles.pageItemRead
                ]}
                onPress={() => visualizarPagina(pagina)}
              >
                <Text style={styles.pageText}>Página {pagina}</Text>
                {progresso.paginas_visualizadas?.includes(pagina) && (
                  <Text style={styles.pageStatus}>✓</Text>
                )}
              </TouchableOpacity>
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

  // Modal de Visualização do PDF
  const ModalPdfViewer = () => (
    <Modal
      animationType="slide"
      transparent={true}
      visible={modalPdfVisible}
      onRequestClose={() => setModalPdfVisible(false)}
    >
      <View style={styles.modalContainer}>
        <View style={styles.pdfModalContent}>
          {/* Header do Modal */}
          <View style={styles.pdfHeader}>
            <Text style={styles.pdfTitle}>
              Página {paginaAtual} de {tarefaSelecionada?.total_paginas}
            </Text>
            <TouchableOpacity
              style={styles.closePdfButton}
              onPress={() => {
                setModalPdfVisible(false);
                setModalProgressoVisible(true);
              }}
            >
              <Text style={styles.closePdfButtonText}>X</Text>
            </TouchableOpacity>
          </View>

          {/* Área do PDF */}
          <View style={styles.pdfContainer}>
            {carregandoPdf ? (
              <View style={styles.loadingContainer}>
                <Text style={styles.loadingText}>Carregando página...</Text>
              </View>
            ) : (
              <View style={styles.pdfWrapper}>
                {/* Aqui você pode integrar com um visualizador de PDF */}
                <Text style={styles.pdfPlaceholder}>
                  Visualizador de PDF - Página {paginaAtual}
                </Text>
                <Text style={styles.pdfUrl}>
                  {getUrlPaginaAtual()}
                </Text>
                
                {/* Controles de Navegação */}
                <View style={styles.navigationControls}>
                  <TouchableOpacity
                    style={[
                      styles.navButton,
                      paginaAtual <= 1 && styles.navButtonDisabled
                    ]}
                    onPress={() => navegarPagina(-1)}
                    disabled={paginaAtual <= 1}
                  >
                    <Text style={styles.navButtonText}>‹ Anterior</Text>
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={[
                      styles.navButton,
                      paginaAtual >= (tarefaSelecionada?.total_paginas || 0) && styles.navButtonDisabled
                    ]}
                    onPress={() => navegarPagina(1)}
                    disabled={paginaAtual >= (tarefaSelecionada?.total_paginas || 0)}
                  >
                    <Text style={styles.navButtonText}>Próxima ›</Text>
                  </TouchableOpacity>
                </View>
              </View>
            )}
          </View>

          {/* Barra de Progresso no Footer */}
          <View style={styles.pdfFooter}>
            <View style={styles.footerProgress}>
              <Text style={styles.footerProgressText}>
                Progresso: {progresso.percentual_concluido || 0}%
              </Text>
              <View style={styles.footerProgressBar}>
                <View 
                  style={[
                    styles.footerProgressFill,
                    { width: `${progresso.percentual_concluido || 0}%` }
                  ]} 
                />
              </View>
            </View>
          </View>
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

          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisa uma tarefa"
            placeholderTextColor="#ffffff"
            value={termoBusca}
            onChangeText={setTermoBusca}
          />
        </View>
        {renderTarefas()}
      </ScrollView>
      
      <ModalProgresso />
      <ModalPdfViewer />
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