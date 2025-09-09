import React, { useState } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import api from '../../../../services/api';

export default function EquipeRanking({ navigation, route}) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const equipe = route.params?.equipe || {}; 
  const usuario = route.params?.usuario;
  const BASE_URL = `${url}/dev4tec/img/`
  
  const [termoBusca, setTermoBusca] = useState('');
  const [dados, setDados] = useState([]);


  //Lista Equipes em ordem de pontuação
  async function listarDados() {
    try {
      const res = await api.get(`dev4tec/ranking.php`, {
      params: {id_equipe: equipe.id_equipe }
    });

    if (res.data.success) {
      setDados(res.data.result || []);
    } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
      }
    }
    catch (error) {
      console.log("Erro ao listar as Equipes", error);
    }
  }

  useEffect(() => {
    listarDados();
  }, [equipe?.id_equipe]);

  //Filtra equipes pela busca
  const filtrarEquipes = () => {
    let equipesFiltradas = dados;
    
    // Aplica filtro de busca
    if (termoBusca) {
      const termo = termoBusca.toLowerCase();
      equipesFiltradas = equipesFiltradas.filter(item => 
        item.nome_equipe.toLowerCase().includes(termo) || 
        item.nome_categoria.toLowerCase().includes(termo)
      );
    }
    return equipesFiltradas;
  };


  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View style={styles.nav}>
          <TouchableOpacity 
            style={styles.botaodevoltar}
            onPress={() => navigation.goBack()}
          >
            <Ionicons name="arrow-back" size={24} color={theme.text} />
          </TouchableOpacity>
            <Text style={styles.titulo}>WORKFLOW</Text>
            <View style={styles.espacoHeader} />
        </View>

        <View style={styles.containertarefas}>
          <Image source={equipe.imagem} style={styles.imag} />
          <View style={styles.textos}>
            <Text style={styles.textolistatitulo}>{equipe.titulo}</Text>
            <Text style={styles.textolistacargo}>{equipe.cargo}</Text>
          </View>
        </View>

        <Text style={styles.titulo2}>Ranking dos membros da equipe</Text>
          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisar funcionário"
            placeholderTextColor="#ffffff"
            value={termoBusca}
            onChangeText={setTermoBusca}
          />

        {dados.map((item)=> {
          const posicaoOriginal = dados.findIndex((d) => d.id_equipe === item.id_equipe);
          <View key={item.FuncionarioId} style={styles.containertarefas}>   
            <Text style={styles.colocacao}>{posicaoOriginal + 1}º</Text>
              <Image 
                source={item.foto_perfil ?  { uri: `${BASE_URL}${item.foto_perfil}?t=${new Date().getTime()}` } : require('../../../../assets/img/image.png')} 
                style={styles.imag} 
              />
            <View style={styles.textos}>
              <Text style={styles.textolistatitulo}>{item.nome}</Text>
              <Text style={styles.textolistacargo}>{item.cargo}</Text>
            </View>
          </View>
        })}
        </ScrollView>
      </View>
  );
}