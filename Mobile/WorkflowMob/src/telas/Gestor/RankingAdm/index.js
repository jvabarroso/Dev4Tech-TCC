import React, { useState, useEffect  } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView } from 'react-native';
import { getStyles } from './style';
import { useFocusEffect } from '@react-navigation/native';
import { useTheme } from '../../../styles/themecontext'
import api from '../../../../services/api';

export default function RankingAdm({route, navigation}){
  const { theme } = useTheme();
  const styles = getStyles(theme);
  
  const usuario = route.params?.usuario;

  const [termoBusca, setTermoBusca] = useState('');
  const [dados, setDados] = useState([]);

  // Atualiza usuário ao voltar para a tela
  useFocusEffect(
    React.useCallback(() => {
      listarDados();
    }, [])
  );

  //Lista Equipes em ordem de pontuação
  async function listarDados() {
    try {
      const res = await api.get(`dev4tech/ranking.php`, {
      params: {id_empresa: usuario.id_empresa }
    });

    if (res.data.success) {
      setDados(res.data.result || []);
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
    listarDados();
  }, [usuario?.id_empresa]);

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

    return(
      <View style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
              <View>
                <Text style={styles.titulo}>Ranking de Equipes</Text>
                <TextInput
                  style={styles.navinput}
                  placeholder="🔍 Pesquisa uma equipe"
                  placeholderTextColor="#ffffff"
                  value={termoBusca}
                  onChangeText={setTermoBusca}
                />
              </View>

            {dados.length === 0 ? (
                      <Text style={{ textAlign: 'center', marginTop: 20 }}>Nenhuma equipe encontrada</Text>
            ) : (
            filtrarEquipes().map((item) => {
              const posicaoOriginal = dados.findIndex((d) => d.id_equipe === item.id_equipe); {/* findIndex percorre todo o array e retorna a posição do primeiro elemento encontrado */}
              return (
              <TouchableOpacity
                onPress={() => navigation.navigate('RankingEstastistico', { equipe: item, posicaoOriginal: posicaoOriginal + 1 })}
                key={item.id_equipe}
              >
                  <View style={styles.containertarefas}>
                    <Text style={styles.colocacao}>{posicaoOriginal + 1}º</Text>
                    <Image 
                      source={item.foto_url ? { uri: item.foto_url } : require('../../../../assets/img/image.png')} 
                      style={styles.imag} 
                    />
                    <View style={styles.textos}>
                      <Text style={styles.textolistatitulo}>{item.nome_equipe}</Text>
                      <Text style={styles.textolistacargo}>{item.nome_categoria}</Text>
                    </View>
                </View>
              </TouchableOpacity>
              );
              })
            )}
          </ScrollView>
        </View>
  );
}