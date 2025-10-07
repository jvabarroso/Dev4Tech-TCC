import React, { useState, useEffect } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import api from '../../../../services/api';
import url from '../../../../services/url';

export default function EquipeRanking({ navigation, route}) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const equipe = route.params?.equipe || {}; 
  const usuario = route.params?.usuario;
  const BASE_URL = `${url}/dev4tech/img/`
  
  const [termoBusca, setTermoBusca] = useState('');
  const [dados, setDados] = useState([]);


  //Lista Equipes em ordem de pontuação
  async function listarDados() {
    try {
      const res = await api.get(`dev4tech/rankingfunc.php`, {
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
  const filtrarFunc = () => {
    let funcionarioFiltrados = dados;
    
    // Aplica filtro de busca
    if (termoBusca) {
      const termo = termoBusca.toLowerCase();
      funcionarioFiltrados = funcionarioFiltrados.filter(item => 
        item.Nome?.toLowerCase().includes(termo) || 
        item.Cargo?.toLowerCase().includes(termo)
      );
    }
    return funcionarioFiltrados;
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
          <Image 
            source={equipe.foto_equipe ? { uri: equipe.foto_equipe } : require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textos}>
            <Text style={styles.textolistatitulo}>{equipe.nome_equipe}</Text>
            <Text style={styles.textolistacargo}>{equipe.nome_categoria}</Text>
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
        {filtrarFunc().map((item, index) => (
          <View key={item.FuncionarioId} style={styles.containertarefas}>   
            <Text style={styles.colocacao}>{index + 1}º</Text>
            <Image 
              source={item.foto_perfil 
                ? { uri: `${BASE_URL}${item.foto_perfil}?t=${new Date().getTime()}` } 
                : require('../../../../assets/img/image.png')} 
              style={styles.imag} 
            />
            <View style={styles.textos}>
              <Text style={styles.textolistatitulo}>{item.Nome}</Text>
              <Text style={styles.textolistacargo}>{item.Cargo}</Text>
            </View>
          </View>
        ))}
        </ScrollView>
      </View>
  );
}