import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, TextInput, ScrollView, ActivityIndicator } from 'react-native';
import { showMessage } from "react-native-flash-message";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';

import api from '../../../../services/api';

export default function Planejamento({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [errorMessage, setErrorMessage] = useState(null);
  const [filtroAtivo, setFiltroAtivo] = useState('pendente');
  const [termoBusca, setTermoBusca] = useState('');
  const [usuarioState, setusuarioState] = useState(usuario);
  const [sucess, setSucess] = useState(false);

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


  async function contarpaginas(tarefa) {   
    if (tarefa.total_paginas && tarefa.total_paginas > 0) {
      console.log("Tarefa já processada, redirecionando para leitura...");
      navigation.navigate('ArquivosPdf', {
        tarefa: tarefa,
        usuario: usuario
      });
      return;
    }
    try {
      const resContagem = await api.post('dev4tech/contar_paginas.php', {
        caminho_arquivo: 'C:/xampp/htdocs/dev4tech/' + tarefa.nome_arquivo
      });

      if (!resContagem.data.success) {
        console.log("ERRO NA CONTAGEM DE PÁGINAS:", resContagem.data.message);
        return;
      }

      const totalPaginas = resContagem.data.total_paginas;

      const res = await api.post('dev4tech/cadastrofunc.php', {
        id_tarefa : tarefa.id_tarefa, 
        nome_arquivo :tarefa.nome_arquivo, 
        total_paginas : totalPaginas, 
        hash_arquivo : tarefa.nome_arquivo, 
      });
      setSucess(true);
      listarDados();
      showMessage({
        message: 'Sucesso.',
        description: `PDF processado com ${totalPaginas} páginas`,
        floating: true,
        statusBarHeight: 70,
        type: "success",
        duration: 2000,             
      })
    } 
    catch (error) {
      console.log("ERRO NA VISUALIZAÇÃO DAS PÁGINAS:", error.message);
      if (error.response) {
        console.log("RESPOSTA DO SERVIDOR:", error.response.data);
      }
      if (error.request) {
        console.log("SEM RESPOSTA, REQUEST:", error.request);
      }
      setSucess(false);
      showMessage({
        message: 'Erro.',
        description: 'Falha ao processar o PDF',
        floating: true,
        statusBarHeight: 70,
        type: "success",
        duration: 2000,             
      })
    }
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

      console.log('Resposta bruta:', res);
      console.log('Dados:', JSON.stringify(res.data, null, 2)); 

      if (res.data.success) {
        // Calcular o status com base na data de entrega
        const tarefasComStatus = res.data.result.map(tarefa => {
          const dataEntrega = new Date(tarefa.data_entrega);
          const hoje = new Date();

          let status = 'pendente';

          return {
            ...tarefa,
            status_tarefa: status,
            pendente: status === 'pendente',
            fazendo: status === 'fazendo',
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
      console.log("Erro ao listar Arquivos:", error);
      setErrorMessage("Erro de conexão com o servidor");
    }
  }

  useEffect(() => {
    listarDados();
  }, [usuarioState?.id]);


  const filtrarArquivos = () => {
    let arquivosFiltradas = dados;
    
    // Aplica filtro por status
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
        // Mostra todas
    }
    
    // Aplica filtro de busca
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
        onPress={() => contarpaginas(item)}
      >
        <View style={styles.linhaTarefa}>
          <Image 
            source={require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textosTarefa}>
            <Text style={styles.textolistatitulo}>{item.nome_arquivo}</Text>
          </View>

          {filtroAtivo === 'concluido'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#adadadff' }]}>
              <Text style={styles.textofiltro}>Entregue</Text>
            </View>: null
          }
          {filtroAtivo === 'fazendo'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#adadadff' }]}>
              <Text style={styles.textofiltro}>Fazendo</Text>
            </View>: null
          }
          {filtroAtivo === 'atrasada'? 
            <View style={[styles.containerfiltro, { backgroundColor: '#adadadff' }]}>
              <Text style={styles.textofiltro}>Atrasado</Text>
            </View>: null
            }
        </View>

        <View style={styles.linhaInfo}>
          <Text style={[styles.textolistacargo, { backgroundColor: corDificuldade(item.dificuldade) }]}>
            12
          </Text>
          <Text style={styles.textolistadata}>Até {formatarData(item.data_entrega)}</Text>
        </View>
      </TouchableOpacity>
    ));
  };

  function limitarTexto(texto, limite) {
    return texto.length > limite ? texto.substring(0, limite) + '...' : texto;
  }


  if (errorMessage) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <Text style={{ color: 'red' }}>{errorMessage}</Text>
        <TouchableOpacity onPress={listarDados}>
          <Text style={{ color: theme.primary }}>Tentar novamente</Text>
        </TouchableOpacity>
      </View>
    );
  }

    // Formata as datas do banco 
  function formatarData(data) {
    if (!data) return "";
    const partes = data.split("-"); // ["0000","00","00"]
    if (partes.length !== 3) return data;
    return `${partes[2]}/${partes[1]}/${partes[0]}`;
  }

  // Define a cor pela dificuldade
  function corDificuldade(dificuldade) {
    switch(dificuldade.toLowerCase()) {
      case 'fácil':
        return '#4CAF50'; 
      case 'médio':
        return '#FFC107'; 
      case 'difícil':
        return '#F44336'; 
    }
}

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
              style={[styles.botao, { backgroundColor: filtroAtivo === 'atrasada' ? '#1A5CFF' : theme.inputBackground }]}
              onPress={() => setFiltroAtivo('atrasada')}
            >
              <Text style={[styles.textobotao, { color: filtroAtivo === 'atrasada' ? '#fff' : theme.text3 }]}>
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
    </View>
  );
}