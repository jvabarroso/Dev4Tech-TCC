import React, { useState, useEffect  } from 'react';
import { Text, View, Image, ScrollView, TextInput } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import api from '../../../../services/api';

export default function Ranking({route, navigation}){
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;

  const [termoBusca, setTermoBusca] = useState('');
  const [dados, setDados] = useState([]);

  //Lista Equipes em ordem de pontuação
  async function listarDados() {
    try {
      const res = await api.get(`dev4tec/ranking.php`, {
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
                <View style={styles.containertarefas} key={item.id_equipe}>
                  <Text style={styles.colocacao}>{posicaoOriginal + 1}º</Text>
                  <Image 
                    source={item.foto_equipe ? { uri: item.foto_equipe } : require('../../../../assets/img/image.png')} 
                    style={styles.imag} 
                  />
                  <View style={styles.textos}>
                    <Text style={styles.textolistatitulo}>{item.nome_equipe}</Text>
                    <Text style={styles.textolistacargo}>{item.nome_categoria}</Text>
                  </View>
                </View>
              );
              })
          )}
          </ScrollView>
        </View>
  );
}
