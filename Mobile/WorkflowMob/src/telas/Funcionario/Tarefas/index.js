import React, { useState, useEffect} from 'react';
import { Text, View, TouchableOpacity, Image, TextInput, ScrollView, ActivityIndicator } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import api from '../../../../services/api';

export default function Tarefas({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState(null);
  const [filtroAtivo, setFiltroAtivo] = useState('pendente'); // 'pendente', 'atrasada', 'concluido'
  const [termoBusca, setTermoBusca] = useState('');


  async function listarDados() {
    console.log("ID do usuário recebido:", usuario?.id);
    if(!usuario?.id) {
        console.log("ID do usuário não disponível");
        return;
    }
    
    try {
      setIsLoading(true);
      setErrorMessage(null);
      const res = await api.get(`dev4tec/tarefa.php`, {
        params: {
          id_funcionario: usuario.id // Use o ID do usuário logado
        }
      });

      if (res.data.success) {
        // Adiciona propriedades de status para facilitar o filtro
        const tarefasFormatadas = res.data.result.map(tarefa => ({
          ...tarefa,
          pendente: tarefa.status_tarefa === 'pendente',
          atrasada: tarefa.status_tarefa === 'atrasada',
          concluido: tarefa.status_tarefa === 'concluido'
        }));
        setDados(tarefasFormatadas);
      } else {
        console.log("Erro na API:", res.data.message);
        setDados([]);
      }
    }
    catch (error) {
      console.log("Erro ao listar tarefas:", error);
      setErrorMessage("Erro de conexão com o servidor");
    }
    finally {
      setIsLoading(false);
    }
    }

    useEffect(() => {
      listarDados();
    }, [usuario?.id]);



  const filtrarTarefas = () => {
    let tarefasFiltradas = dados;
    
    // Aplica filtro por status
    switch(filtroAtivo) {
      case 'pendente':
        tarefasFiltradas = tarefasFiltradas.filter(item => item.status_tarefa === 'pendente');
        break;
      case 'atrasada':
        tarefasFiltradas = tarefasFiltradas.filter(item => item.status_tarefa === 'atrasada');
        break;
      case 'concluido':
        tarefasFiltradas = tarefasFiltradas.filter(item => item.status_tarefa === 'concluido');
        break;
      default:
        // Mostra todas se nenhum filtro estiver ativo
    }
    
    // Aplica filtro de busca se houver termo
    if (termoBusca) {
      const termo = termoBusca.toLowerCase();
      tarefasFiltradas = tarefasFiltradas.filter(item => 
        item.nomeTarefa.toLowerCase().includes(termo) || 
        item.instrucoes.toLowerCase().includes(termo)
      );
    }
    
    return tarefasFiltradas;
  };

 const renderTarefas = () => {
    const tarefasFiltradas = filtrarTarefas();
    
    if (tarefasFiltradas.length === 0) {
      return (
        <Text style={[styles.texto, { textAlign: 'center', marginTop: 20 }]}>
          Nenhuma tarefa {filtroAtivo} encontrada
        </Text>
      );
    }

    return tarefasFiltradas.map((item, index) => (
      <TouchableOpacity
        key={`${item.id_tarefa}-${index}`} 
        onPress={() => navigation.navigate('TarefaEnvio', { tarefas: item })}
        style={styles.containertarefas}
      >
        <View style={styles.linhaTarefa}>
          <Image 
            source={item.imagem ? { uri: item.imagem } : require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textosTarefa}>
            <Text style={styles.textolistatitulo}>{item.nomeTarefa}</Text>
            <Text style={styles.textolista}>{limitarTexto(item.instrucoes, 23)}</Text>
          </View>
        </View>

        <View style={styles.linhaInfo}>
          <Text style={styles.textolistacargo}>{item.dificuldade}</Text>
          <Text style={styles.textolistadata}>{item.data_entrega}</Text>
        </View>
      </TouchableOpacity>
    ));
  };


  function limitarTexto(texto, limite) {
    return texto.length > limite ? texto.substring(0, limite) + '...' : texto;
  }

  if (isLoading) {
    return (
      <View style={[styles.container, { justifyContent: 'center', alignItems: 'center' }]}>
        <ActivityIndicator size="large" color={theme.primary} />
      </View>
    );
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

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
          <View>
            <Text style={styles.titulo}>Tarefas</Text>

            <View style={styles.areabotao}>
              <TouchableOpacity
                style={[styles.botao, { backgroundColor: filtroAtivo === 'pendente' ? '#1A5CFF' : theme.inputBackground }]}
                onPress={() => setFiltroAtivo('pendente')}
              >
                <Text style={[styles.textobotao, { color: filtroAtivo === 'pendente' ? theme.text4 : theme.text }]}>Pendente</Text>
              </TouchableOpacity>

              <TouchableOpacity
                style={[styles.botao, { backgroundColor: filtroAtivo === 'atrasada' ? '#1A5CFF' : theme.inputBackground }]}
                onPress={() => setFiltroAtivo('atrasada')}
              >
                <Text style={[styles.textobotao, { color: filtroAtivo === 'atrasada' ? theme.text4 : theme.text }]}>Atrasados</Text>
              </TouchableOpacity>

              <TouchableOpacity
                style={[styles.botao, { backgroundColor: filtroAtivo === 'concluido' ? '#1A5CFF' : theme.inputBackground }]}
                onPress={() => setFiltroAtivo('concluido')}
              >
                <Text style={[styles.textobotao, { color: filtroAtivo === 'concluido' ? theme.text4 : theme.text }]}>Concluídos</Text>
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