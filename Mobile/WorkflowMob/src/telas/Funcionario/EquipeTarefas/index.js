import React, { useState, useEffect } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useFocusEffect } from '@react-navigation/native';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

import api from '../../../../services/api';


export default function EquipeTarefas({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const equipe = route.params?.equipe || {}; 
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

  //Lista Tarefas
  async function listarDados() {
    if (!equipe?.id_equipe) {
      console.log("ID da Equipe não disponível");
      return;
    }
    
    try {
      setErrorMessage(null);
      const res = await api.get(`dev4tech/tarefaadm.php`, {
        params: { id_equipe: equipe.id_equipe }
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
        onPress={() => navigation.navigate('TarefaEnvio', { tarefa: item })}
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
          <Text style={styles.textolistadata}>{item.data_entrega}</Text>
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

        <View style={styles.containertarefas2}>
          <Image 
            source={equipe.foto_equipe ? { uri: equipe.foto_equipe } : require('../../../../assets/img/image.png')} 
            style={styles.imag} 
          />
          <View style={styles.textos}>
            <Text style={styles.textolistatitulo}>{equipe.nome_equipe}</Text>
            <Text style={styles.textolistacargo}>{equipe.nome_categoria}</Text>
          </View>
        </View>

        <Text style={styles.titulo2}>Tarefas</Text>

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
            <Text style={[styles.textobotao, { color: filtroAtivo === 'concluido' ? theme.text4 : theme.text }]}>Completados</Text>
          </TouchableOpacity>
        </View>

        <TextInput
          style={styles.navinput}
          placeholder="🔍 Pesquisa uma tarefa"
          placeholderTextColor="#ffffff"
        />
        {renderTarefas()}
      </ScrollView>
    </View>
  );
}