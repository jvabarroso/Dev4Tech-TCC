import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, TextInput, ScrollView, ActivityIndicator } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';
import {Ionicons} from '@expo/vector-icons';

import api from '../../../../services/api';

export default function EquipesTarefasAdm({ navigation, route }) {

  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [usuarioState, setusuarioState] = useState(usuario);

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
          const entregue = +tarefa.entregue;
          const dataEntrega = new Date(tarefa.data_entrega);
          const hoje = new Date();

          let status = 'pendente';
          if (entregue === 0) {
            status = 'Enviada';
          } else if (dataEntrega < hoje) {
            status = 'Atrasada';
          } else {
            status = 'Pendente';
          }

          return {
            ...tarefa,
            entregue,
            status_tarefa: status,
            pendente: status === 'pendente',
            atrasada: status === 'atrasada',
            concluido: status === 'concluido'
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
          
          {dados.map((item, index) => (
            <View key={index} style={styles.containertarefas}>
                <View style={styles.linhaTarefa}>
                <Image 
                    source={require('../../../../assets/img/image.png')} 
                    style={styles.imag} 
                />
                <View style={styles.textosTarefa}>
                    <Text style={styles.textolistatitulo}>Tarefa: {item.nomeTarefa}</Text>
                    <Text style={styles.textolista}>Equipe: {item.nome_equipe}</Text>
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
            </View>
          ))}
        </View>
      </ScrollView>
    </View>
  );
}