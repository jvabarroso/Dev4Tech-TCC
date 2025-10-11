import React, { useState, useEffect } from 'react'; 
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView,  ActivityIndicator} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { useFocusEffect } from '@react-navigation/native';
import { Ionicons } from '@expo/vector-icons';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import api from '../../../../services/api';
import url from '../../../../services/url';

export default function EquipesAdm({ route, navigation }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);
  
  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [equipeSelecionada, setEquipeSelecionada] = useState(null)
  const [errorMessage, setErrorMessage] = useState(null);
  const [termoBusca, setTermoBusca] = useState('');
  const [usuarioState, setusuarioState] = useState(usuario);

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


  //Lista Equipes
  async function listarDados() {
  if (!usuario?.AdminId) {
        console.log("ID do usuário não disponível");
        return;
  }
  
  try {
    setErrorMessage(null);
    const res = await api.get(`dev4tech/equipeadm.php`, {
      params: {
        id_administrador: usuario.AdminId // Use o ID do usuário logado
      }
    });

    if (res.data.success) {
      setDados(res.data.result || []);
    } else {
      console.log("Erro na API:", res.data.message);
      setDados([]);
    }
  }
  catch (error) {
    console.log("Erro ao listar equipes:", error);
    setErrorMessage("Erro de conexão com o servidor");
  }
  }

  useEffect(() => {
    listarDados();
  }, [usuarioState?.AdminId]);

  const toggleEquipe = (AdminId) => {
    setEquipeSelecionada(equipeSelecionada === AdminId ? null : AdminId);
  };


  if (errorMessage) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <Text style={{ color: 'red' }}>{errorMessage}</Text>
        <TouchableOpacity onPress={listarDados}>
          <Text>Tentar novamente</Text>
        </TouchableOpacity>
      </View>
    );
  }

  const filtrarEquipes = () => {
    let tarefasFiltradas = dados;
    
    // Aplica filtro de busca
    if (termoBusca) {
      const termo = termoBusca.toLowerCase();
      tarefasFiltradas = tarefasFiltradas.filter(item => 
        item.nome_equipe.toLowerCase().includes(termo) || 
        item.nome_categoria.toLowerCase().includes(termo)
      );
    }
    
    return tarefasFiltradas;
  };


  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
          <Text style={styles.titulo}>Equipes</Text>
          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisa uma equipe"
            placeholderTextColor="#ffffff"
            value={termoBusca}
            onChangeText={setTermoBusca}
          />
        {dados.length === 0 ? (
          <Text style={{ textAlign: 'center', marginTop: 20 }}>Nenhuma equipe encontrada</Text>
        ) : (
          filtrarEquipes().map(item => (
          <View key={item.id_equipe}>
              <TouchableOpacity
                style={styles.containertarefas}
                onPress={() => toggleEquipe(item.id_equipe)}
              >
                <Image 
                  source={item.foto_url ? { uri: item.foto_url } : require('../../../../assets/img/image.png')} 
                  style={styles.imag} 
                />
                <View style={styles.areatextobotao}>
                    <View style={styles.textos}>
                    <Text style={styles.textolistatitulo}>{item.nome_equipe}</Text>
                    <Text style={styles.textolistacargo}>{item.nome_categoria}</Text>
                    </View>
                    <TouchableOpacity
                        style={styles.botaoeditar}
                        onPress={() => navigation.navigate('EditEquipe', { equipe: { ...item, id_empresa: item.id_empresa || usuario.id_empresa } })}
                    >
                        <Ionicons name="create-outline" size={35} color={theme.text}/>
                    </TouchableOpacity>                    
                </View>

              </TouchableOpacity>
                {equipeSelecionada === item.id_equipe && (
                <View style={styles.areacard}>

                  <TouchableOpacity 
                    onPress={() => navigation.navigate('Chatadm', { 
                      equipe: item, 
                      usuario: usuarioState  
                    })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/chat.jpg')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Geral</Title>
                        <Paragraph style={styles.paragraph}>Veja as mensagens da equipe</Paragraph>
                      </Card.Content>
                    </Card>

                  </TouchableOpacity>

                  <TouchableOpacity 
                    onPress={() => navigation.navigate('EquipesTarefasAdm', { 
                      equipe: item, 
                      usuario: usuarioState  
                    })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/tarefas.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Tarefas</Title>
                        <Paragraph style={styles.paragraph}>Avalie as tarefas enviadas</Paragraph>
                      </Card.Content>
                    </Card>

                  </TouchableOpacity>

                  <TouchableOpacity
                    onPress={() => navigation.navigate('RankingAdm')}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/ranking.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Ranking</Title>
                        <Paragraph style={styles.paragraph}>Veja a posição das Equipes e seus dados</Paragraph>
                      </Card.Content>
                    </Card>

                  </TouchableOpacity>

                  <TouchableOpacity
                      onPress={() => navigation.navigate('EquipeFuncionarioAdm', { 
                      equipe: item, 
                      usuario: usuarioState  
                    })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/equipes.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Membros</Title>
                        <Paragraph style={styles.paragraph}>Veja os membros da Equipe</Paragraph>
                      </Card.Content>
                    </Card>
                  </TouchableOpacity>

                  <TouchableOpacity>
                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/kanban.webp')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Planejamento</Title>
                        <Paragraph style={styles.paragraph}>KanBan e Scrum</Paragraph>
                      </Card.Content>
                    </Card>
                  </TouchableOpacity>

                </View>
              )}
            </View>
            )
          ))}
      </ScrollView>
    </View>
  );
}
