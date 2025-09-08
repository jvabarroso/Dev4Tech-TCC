import React, { useState, useEffect } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import api from '../../../../services/api';

export default function EquipeFuncionario({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);
      
  const equipe = route.params?.equipe || {}; 
  console.log("Equipe:", equipe);

  const [dados, setDados] = useState([]);
  const BASE_URL = 'http://10.239.0.124/dev4tec/img/'
  
  //Lista os funcionarios
  async function listarFuncionarios() {
    try {
      const res = await api.get(`dev4tec/funcionario.php`, {
        params: {
          id_equipe: equipe.id_equipe
        }
    });
            
    if (res.data.success) {
      setDados(res.data.result || []);
      console.log("Dados dos funcionários:", res.data.result);
      console.log("Foto do primeiro funcionário:", res.data.result[0]?.foto_perfil);
    } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
      }
    }
    catch (error) {
      console.log("Erro ao listar categorias", error);
    }
  }


  useEffect(() => {
    listarFuncionarios();
  }, [equipe?.id_equipe]);



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

        <Text style={styles.titulo2}>Procurar membros da equipe</Text>
          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisar funcionário"
            placeholderTextColor="#ffffff"
          />

        {dados.map(item => (
          <View key={item.FuncionarioId} style={styles.containertarefas}>
            <Image 
              source={item.foto_perfil ? { uri: `${BASE_URL}${item.foto_perfil}?t=${new Date().getTime()}` } : require('../../../../assets/img/image.png')} 
              style={styles.imag} 
            />
            <View style={styles.textos}>
              <Text style={styles.textolistatitulo}>{item.nome}</Text>
              <Text style={styles.textolistacargo}>{item.cargo}</Text>
            </View>
          </View>
        ))}
        </ScrollView>
      </View>
  );
}