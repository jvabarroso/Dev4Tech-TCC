import React, { useState, useEffect  } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView } from 'react-native';
import { useFocusEffect } from '@react-navigation/native';
import { Dropdown } from 'react-native-element-dropdown';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import api from '../../../../services/api';
import url from '../../../../services/url';
import fonts from "../../../styles/fonts";

export default function Ranking({route, navigation}){
  const { theme } = useTheme();
  const styles = getStyles(theme);
  
  const usuario = route.params?.usuario;

  const [termoBusca, setTermoBusca] = useState('');
  const [filtroAtivo, setFiltroAtivo] = useState('todas');
  const [dados, setDados] = useState([]);
  const [categoria, setCategoria] = useState([]);
  const [categoriaSelecionada, setCategoriaSelecionada] = useState(null);

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


//Buscar Categorias
    async function listarcategorias() {
      try {
        const res = await api.get(`dev4tech/categoria.php`, {
        params: {id_empresa: usuario.id_empresa }
      });

      if (res.data.success) {
        setCategoria(res.data.result || []);
      } else {
        console.log("Erro na API:", res.data.message);
        setCategoria([]);
      }
      }catch (error) {
          console.log("Erro ao listar categorias", error);
      }
    }

    useEffect(() => {
        listarcategorias();
    }, [usuario?.id_empresa]);


  //Filtra equipes pela busca
  const filtrarEquipes = () => {
    let equipesFiltradas = dados;

    // Aplica filtro por categoria
    if (filtroAtivo === 'categoria' && categoriaSelecionada) {
      equipesFiltradas = equipesFiltradas.filter(item => 
        item.id_categoria === categoriaSelecionada
      );
    }
    
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

  const renderEquipes = () => {
    const equipesFiltradas = filtrarEquipes();
    
    if (equipesFiltradas.length === 0) {
      return (
        <Text style={[styles.textos, { textAlign: 'center', marginTop: 20 }]}>
          Nenhuma Equipe encontrada
        </Text>
      );
    }

    // Para listas filtradas
    return equipesFiltradas.map((item) => {
      const posicaoOriginal = dados.findIndex((d) => d.id_equipe === item.id_equipe); {/* findIndex percorre todo o array e retorna a posição do primeiro elemento encontrado */}
        return (
          <View style={styles.containertarefas} key={item.id_equipe || index}>
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
      );})
    }

  // Função para renderizar cada item do dropdown
  const renderItem = (item, selected) => {
    const isSelected = item.id_categoria === categoriaSelecionada;
    
    return (
      <View style={{
        padding: 15,
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
        backgroundColor: selected ? theme.primaryLight : 'transparent'
      }}>
        <Text style={{
          fontSize: 13,
          fontFamily: fonts.text,
          color: isSelected ? '#000000' : '#FFFFFF'
        }}>
          {item.nome_categoria}
        </Text>
      </View>
    );
  };


    return(
      <View style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
          <View>
            <Text style={styles.titulo}>Ranking de Equipes</Text>
            <View style={styles.areabotao}>
              <TouchableOpacity
                style={[styles.botao, { backgroundColor: filtroAtivo === 'todas' ? '#1A5CFF' : theme.inputBackground }]}
                onPress={() => {
                  setFiltroAtivo('todas');
                  setCategoriaSelecionada(null);
                  setCategoriaSelecionada('');
                }}
              >
                <Text style={[styles.textobotao, { color: filtroAtivo === 'todas' ? theme.text4 : theme.text }]}>Total</Text>
              </TouchableOpacity>
              <Dropdown
                style={[styles.botao, { backgroundColor: filtroAtivo === 'categoria' ? '#1A5CFF' : theme.inputBackground }]}
                data={categoria}
                labelField="nome_categoria" 
                valueField="id_categoria"
                placeholder={"Categorias"}
                value={categoriaSelecionada}
                onPress={() => setFiltroAtivo('categoria')}
                onChange={item => {
                  setFiltroAtivo('categoria');
                  setCategoriaSelecionada(item.id_categoria);
                }}
                placeholderStyle={{ 
                  color: filtroAtivo === 'categoria' ? theme.text4 : theme.text, 
                  fontSize: 15, 
                  fontFamily: fonts.text, 
                }}
                selectedTextStyle={{ 
                  color: filtroAtivo === 'categoria' ? theme.text4 : theme.text, 
                  fontSize: 15, 
                  fontFamily: fonts.text, 
                }}  
                renderItem={(item) => renderItem(item, false)}
                  containerStyle={{
                  backgroundColor: '#1A5CFF',
                  borderRadius: 15,
                }}
                itemTextStyle={{
                  color: "#00000",
                  fontSize: 13,
                  fontFamily: fonts.text,
                }}
                selectedStyle={{
                  color: filtroAtivo === 'categoria' ? theme.text4 : theme.text,
                  fontSize: 13,
                  fontFamily: fonts.text,
                }}
                activeColor={theme.inputBackground} 
                  flatListProps={{
                  showsVerticalScrollIndicator: false,
                  style: { maxHeight: 200 } // Limitar altura máxima da lista
                }}
                />
            </View>

            <TextInput
              style={styles.navinput}
              placeholder="🔍 Pesquisa uma equipe"
              placeholderTextColor="#ffffff"
              value={termoBusca}
              onChangeText={setTermoBusca}
            />
          </View>
          {renderEquipes()}
        </ScrollView>
      </View>
  );
}