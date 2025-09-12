import React, { useState, useEffect } from 'react';
import { Text, View, TouchableOpacity, Image, TextInput, ScrollView, ActivityIndicator } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { useFocusEffect } from '@react-navigation/native';

import api from '../../../../services/api';

export default function Tarefas({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const usuario = route.params?.usuario;
  const [dados, setDados] = useState([]);
  const [errorMessage, setErrorMessage] = useState(null);
  const [filtroAtivo, setFiltroAtivo] = useState('pendente');
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

  async function listarDados() {
    if (!usuario?.id) {
      console.log("ID do usuário não disponível");
      return;
    }
    
    try {
      setErrorMessage(null);
      const res = await api.get(`dev4tec/tarefa.php`, {
        params: { id_funcionario: usuario.id }
      });

      console.log('Resposta bruta:', res);
      console.log('Dados:', JSON.stringify(res.data, null, 2)); 

      if (res.data.success) {
        // Calcular o status com base na data de entrega
        const tarefasComStatus = res.data.result.map(tarefa => {
          const entregue = Boolean(+tarefa.entregue);
          const dataEntrega = new Date(tarefa.data_entrega);
          const hoje = new Date();

          let status = 'pendente';
          if (entregue) {
            status = 'concluido';
          } else if (dataEntrega < hoje) {
            status = 'atrasada';
          } else {
            status = 'pendente';
          }

          return {
            ...tarefa,
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
      setErrorMessage("Erro de conexão com o servidor");
    }
  }

  useEffect(() => {
    listarDados();
  }, [usuarioState?.id]);


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
        // Mostra todas
    }
    
    // Aplica filtro de busca
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
        onPress={() => navigation.navigate('TarefaEnvio', { tarefa: item, usuario: usuario, filtroAtivo: filtroAtivo })}
        style={styles.containertarefas}
      >
        <View style={styles.linhaTarefa}>
          <Image 
            source={require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textosTarefa}>
            <Text style={styles.textolistatitulo}>{item.nomeTarefa}</Text>
            <Text style={styles.textolista}>{limitarTexto(item.instrucoes, 23)}</Text>
          </View>
        </View>

        <View style={styles.linhaInfo}>
          <Text style={styles.textolistacargo}>
            {item.dificuldade} {/* Mostra os ícones de dificuldade */}
          </Text>
          <Text style={styles.textolistadata}>{formatarData(item.data_entrega)}</Text>
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