import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, Modal, TextInput } from 'react-native';
import { showMessage } from "react-native-flash-message";
import * as FileSystem from 'expo-file-system/legacy'; 
import * as Sharing from 'expo-sharing';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';
import {Ionicons} from '@expo/vector-icons';

import url from '../../../../services/url';
import api from '../../../../services/api';

export default function EquipesTarefasAdm({ navigation, route }) {

  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [usuarioState, setusuarioState] = useState(usuario);
  const [modalJustificativa, setModalJustificativa] = useState(false);
  const [tarefaSelecionada, setTarefaSelecionada] = useState(null);

  console.log("Dados do usuario: ",usuario)

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


  // Verifica se a tarefa está atrasada
  function verificarAtraso(item) {
    if (!item.data_limite_entrega || !item.data_envio_entrega) return false;
    
    const dataLimite = new Date(item.data_limite_entrega);
    const dataEnvio = new Date(item.data_envio_entrega);
    
    return dataEnvio > dataLimite;
  }

  // Verifica se tem problema relatado
  function temProblemaRelatado(item) {
    return item.problema_relatado && item.problemas && item.problemas.length > 0;
  }
  
  // Conta quantos problemas a tarefa tem
  function contarProblemas(item) {
    return item.problemas ? item.problemas.length : 0;
  }

  //Listar tarefas enviadas para avaliação
  async function listarDados() {
    if (!usuario?.AdminId) {
      console.log("ID do usuário não disponível");
      return;
    }
    
    try {
      const res = await api.get(`dev4tech/tarefaavaliacao.php`, {
        params: { AdminId: usuario.AdminId }
      });

      console.log('Resposta bruta:', res);
      console.log('Dados:', JSON.stringify(res.data, null, 2)); 

      if (res.data.success) {
        const tarefasComStatus = res.data.result.map(tarefa => {
          return {
            ...tarefa,
            statusAvaliacao: null,
            atrasoJustificado: false
          };
        });
        
        setDados(tarefasComStatus);
      } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
      }
    }
    catch (error) {
      console.log("Erro ao listar tarefas:", error);
    }
  }

  useEffect(() => {
    listarDados();
  }, [usuarioState?.id]);

  // Abrir modal de justificativa
  function abrirModalJustificativa(item, index) {
    setTarefaSelecionada({ item, index });
    setModalJustificativa(true);
  }

  // Confirmar justificativa
  function confirmarJustificativa() {
    if (!tarefaSelecionada) return;

    const novosDados = [...dados];
    novosDados[tarefaSelecionada.index].atrasoJustificado = true;
    
    setDados(novosDados);
    setModalJustificativa(false);
    setTarefaSelecionada(null);
  }


  //Post para avaliar tarefa
  async function avaliar(item) {  
    try {
      const estaAtrasado = verificarAtraso(item);
      
      let pontos = 0;
      
      if (item.statusAvaliacao === 'aceito') {
        if (!estaAtrasado || item.atrasoJustificado) {
          switch(item.dificuldade) {
            case 'Fácil': pontos = 2; break;
            case 'Médio': pontos = 4; break;
            case 'Difícil': pontos = 6; break;
          }
        } else {
          pontos = -5;
        }
      } else if (item.statusAvaliacao === 'negado') {
        pontos = -3;
      }

      const obj = {
        id_entrega: item.id_entrega,
        statusAvaliacao: item.statusAvaliacao,
        dificuldade: item.dificuldade,
        id_funcionario: item.FuncionarioId,
        pontos: pontos,
        atraso_justificado: item.atrasoJustificado,
      };

      console.log('Dados enviados para edição:', obj);

      const res = await api.post('dev4tech/tarefaconfirmacao.php', obj, {
        headers: {
          'Content-Type': 'application/json',
        }
      });

      console.log('Resposta da API:', res.data); 
         
      if (res.data.success) {
        showMessage({
          message: 'Sucesso.',
          description: 'Dados avaliados com sucesso',
          floating: true,
          statusBarHeight: 70,
          type: "success",
          duration: 2000,             
      });

      setDados(prev => prev.filter(tarefa => tarefa.id_entrega !== item.id_entrega));

      } else {
          showMessage({
            message: 'Erro.',
            description: res.data.message || "Erro ao avaliar os dados",
            floating: true,
            statusBarHeight: 70,
            type: "warning",
            duration: 2000,             
          });
        }
  } catch (error) {
      console.error("Erro completo:", error);
        showMessage({
          message: 'Erro.',
          description: "Não foi possível conectar ao servidor",
          floating: true,
          statusBarHeight: 70,
          type: "danger",
          duration: 2000,             
        });
    }
  }

  //Abre o arquivo
  async function abrirArquivo(nome_arquivo) {

  try {
    const fileUrl = `${url}/dev4tech/arquivos/${encodeURIComponent(nome_arquivo)}`;

    const localPath = `${FileSystem.cacheDirectory}${nome_arquivo}`;
    const { uri } = await FileSystem.downloadAsync(fileUrl, localPath);

    const nomeLimpo = nome_arquivo
      .replace(/^[\d\-_]+/, '') 
      .replace(/_/g, ' ')      
      .trim();
  
    const available = await Sharing.isAvailableAsync();
    if (available) {
      await Sharing.shareAsync(uri, { dialogTitle: nomeLimpo });
    } else {
      showMessage({
        message: 'Não foi possível abrir o arquivo',
        description: 'Compartilhamento não disponível neste dispositivo',
        statusBarHeight: 70,
        type: 'danger',
        floating: true,
        duration: 2000,    
      });
    }
  } catch (error) {
    console.log('Erro ao abrir arquivo:', error);
    showMessage({
      message: 'Erro ao abrir arquivo',
      description: 'Verifique se o arquivo existe no servidor ou tente novamente.',
      statusBarHeight: 70,
      type: 'danger',
      floating: true,
      duration: 2000,           
    });
  }
}


  function limitarTexto(texto, limite) {
    return texto.length > limite ? texto.substring(0, limite) + '...' : texto;
  }


  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View>
          <View style={styles.nav}>
            <TouchableOpacity 
              style={styles.botaodevoltar}
              onPress={() => navigation.goBack()}
            >
              <Ionicons name="arrow-back" size={25} color={theme.text} />
            </TouchableOpacity>
            <Image 
              style={styles.tituloi}
              source={theme.logo} >
            </Image>
            <View style={styles.espacoHeader} />
          </View>

          <Text style={styles.titulo}>Tarefas</Text>
          <Text style={styles.subtitulo}>Últimas tarefas</Text>
          
          {dados.map((item, index) => {
            const estaAtrasado = verificarAtraso(item);
            const temProblema = temProblemaRelatado(item);

            return(
            <View key={index} style={styles.containertarefas}>

                <View style={styles.linhaTarefa}>
                  <Image 
                      source={require('../../../../assets/img/image.png')} 
                      style={styles.imag} 
                  />
                  <View style={styles.textosTarefa}>
                      <Text style={styles.textolistatitulo}>Tarefa: {item.nomeTarefa}</Text>
                      <Text style={styles.textolista}>Equipe: {item.nome_equipe}</Text>
                      {temProblema && (
                        <Text style={[styles.textolista, { color: '#f11919ff' }]}>
                          ⚠️ Tem problema relatado
                        </Text>
                      )}
                  </View>
                </View>

                <View style={styles.linhaInfo}>
                  <Text style={styles.textolistacargo}>Dificuldade: {item.dificuldade}</Text>
                  
                  <View style={styles.linhaBotoes}>
                    <TouchableOpacity
                      style={[
                        styles.botao,
                        item.statusAvaliacao === 'aceito'
                          ? { backgroundColor: '#4CAF50' }
                          : { backgroundColor: '#E0E0E0' }
                      ]}
                      onPress={() => {
                        const novos = [...dados];
                        novos[index].statusAvaliacao =
                          item.statusAvaliacao === 'aceito' ? null : 'aceito';
                        setDados(novos);
                      }}
                      >
                      <Text style={[styles.textoBotao,{ color: item.statusAvaliacao === 'aceito' ? '#fff' : '#000' }]}>
                        Aceitar
                      </Text>
                    </TouchableOpacity>

                    <TouchableOpacity
                      style={[
                        styles.botao,
                        item.statusAvaliacao === 'negado'
                          ? { backgroundColor: '#E53935' }
                          : { backgroundColor: '#E0E0E0' }
                      ]}
                      onPress={() => {
                        const novos = [...dados];
                        novos[index].statusAvaliacao =
                          item.statusAvaliacao === 'negado' ? null : 'negado';
                        setDados(novos);
                      }}
                    >
                      <Text style={[styles.textoBotao,{ color: item.statusAvaliacao === 'negado' ? '#fff' : '#000' }]}>
                        Negar
                      </Text>
                    </TouchableOpacity>
                  </View>
                </View>

                {estaAtrasado && (
                  <View style={styles.linhaInfo}>
                    <Text style={styles.textolistacargo}>Status do Atraso: </Text>
                    {item.atrasoJustificado ? (
                      <Text style={[styles.textolistacargo, { color: '#4CAF50' }]}>
                        Justificado ✓
                      </Text>
                    ) : (
                      <TouchableOpacity 
                        onPress={() => abrirModalJustificativa(item, index)}
                        style={[styles.botao, { backgroundColor: '#FF9800' }]}
                      >
                        <Text style={[styles.textoBotao, { color: '#fff' }]}>
                          {temProblema ? 'Ver Problema' : 'Justificar Atraso'}
                        </Text>
                      </TouchableOpacity>
                    )}
                  </View>
                )}

                <View style={styles.linhaInfo}>
                <Text style={styles.textolistacargo}>Arquivo: </Text>
                <TouchableOpacity onPress={() => abrirArquivo(item.nome_arquivo)}>
                  <Text style={[styles.textolistacargo, { color: '#1C58F2' }]}>
                    {limitarTexto(item.nome_arquivo, 12)}
                  </Text>
                </TouchableOpacity>

                  <TouchableOpacity
                    style={styles.botao}
                    onPress={() => avaliar(item)}
                  >
                    <Text style={[styles.textoBotao,{ color:'#000000ff' }]}> Confirmar</Text>
                  </TouchableOpacity>
                </View>
            </View>)
          })}
        </View>
      </ScrollView>
      <Modal
        visible={modalJustificativa}
        animationType="slide"
        transparent={true}
        onRequestClose={() => setModalJustificativa(false)}
      >
        <View style={styles.modalOverlay}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>
              {tarefaSelecionada?.item?.problema_relatado ? 'Problemas Relatados' : 'Justificar Atraso'}
            </Text>
            
            {tarefaSelecionada?.item?.problema_relatado ? (
              <View>
                <Text style={styles.problemaTitle}>
                  {tarefaSelecionada.item.problemas.length} problema(s) relatado(s) pela equipe:
                </Text>
                
                <ScrollView style={styles.problemasScroll} showsVerticalScrollIndicator={true}>
                  {tarefaSelecionada.item.problemas.map((problema, index) => (
                    <View key={problema.idProblema} style={styles.problemaContainer}>
                      <Text style={styles.problemaIndex}>Problema {index + 1}:</Text>
                      <Text style={styles.problemaText}>
                        {problema.descricao}
                      </Text>
                    </View>
                  ))}
                </ScrollView>
                
                <Text style={styles.instrucoesText}>
                  Se estes problemas justificam o atraso, clique em "Aceitar Justificativa" para que o funcionário não perca pontos.
                </Text>
              </View>
            ) : (
              <Text style={styles.instrucoesText}>
                Não há problemas relatados para esta tarefa. Você pode justificar o atraso manualmente se necessário.
              </Text>
            )}

            <View style={styles.modalButtons}>
              <TouchableOpacity 
                style={[styles.modalButton, { backgroundColor: '#E0E0E0' }]}
                onPress={() => setModalJustificativa(false)}
              >
                <Text style={styles.modalButtonText}>Fechar</Text>
              </TouchableOpacity>
              
              {tarefaSelecionada?.item?.problema_relatado && (
                <TouchableOpacity 
                  style={[styles.modalButton, { backgroundColor: '#4CAF50' }]}
                  onPress={confirmarJustificativa}
                >
                  <Text style={[styles.modalButtonText, { color: '#fff' }]}>
                    Aceitar Justificativa
                  </Text>
                </TouchableOpacity>
              )}
            </View>
          </View>
        </View>
      </Modal>
    </View>
  );
}