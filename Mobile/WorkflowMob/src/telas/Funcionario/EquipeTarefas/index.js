import React, { useState } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

export default function EquipeTarefas({ navigation, route }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const { equipe } = route.params;
  
  const [tarefas, setTarefas] = useState([
    {
      id: '1',
      titulo: 'Tarefa 1',
      descricao: 'lorem ipsum kwkkwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww',
      cargo: 'Desenvolvimento de Software',
      datadeentrega: '16/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      pendente: true,
      atrasado: false,
      completado: false,
      imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
      id: '2',
      titulo: 'Tarefa 2',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Documentação',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: true,
      pendente: false,
      atrasado: true,
      completado: false,
      imagem: require('../../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Tarefa 3',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Design',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      pendente: false,
      atrasado: false,
      completado: true,
      imagem: require('../../../../assets/img/image.png'),
    },
    {
      id: '4',
      titulo: 'Tarefa 4',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Back-end',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      pendente: false,
      atrasado: false,
      completado: true,
      imagem: require('../../../../assets/img/image.png'),
    },
  ]);
    
    const [tarefaspendentes, setTarefaspendentes] = useState(true);
    const [tarefasatrasadas, setTarefasatrasadas] = useState(false);
    const [tarefascompletados, setTarefascompletados] = useState(false);

    const botaopendente = () => {
      const tarefa = tarefas.find(
          (item) => item.pendente === true
        );
        if (tarefa) {
          setTarefaspendentes(true); 
          setTarefasatrasadas(false); 
          setTarefascompletados(false); 
        } 
    }

    const botaoatrasado = () => {
      const tarefa = tarefas.find(
          (item) => item.atrasado === true
        );
        if (tarefa) {
          setTarefaspendentes(false); 
          setTarefasatrasadas(true); 
          setTarefascompletados(false); 
        } 
    }

    const botaocompletado = () => {
      const tarefa = tarefas.find(
          (item) => item.completado === true
        );
        if (tarefa) {
          setTarefaspendentes(false); 
          setTarefasatrasadas(false); 
          setTarefascompletados(true);  
        } 
    }
  const renderTarefas = (filtro) =>
    tarefas.filter(filtro).map((item) => (
      <TouchableOpacity
        key={item.id}
        onPress={() => navigation.navigate('TarefaEnvio', { tarefas: item })}
        style={styles.containertarefas}
      >
        <View style={styles.linhaTarefa}>
          <Image source={item.imagem} style={styles.imag} />
          <View style={styles.textosTarefa}>
            <Text style={styles.textolistatitulo}>{item.titulo}</Text>
            <Text style={styles.textolista}>{limitarTexto(item.descricao, 23)}</Text>
          </View>
        </View>

        <View style={styles.linhaInfo}>
          <Text style={styles.textolistacargo}>{item.cargo}</Text>
          <Text style={styles.textolistadata}>{item.datadeentrega}</Text>
        </View>
      </TouchableOpacity>
  ));
  
  function limitarTexto(texto, limite) {
    return texto.length > limite ? texto.substring(0, limite) + '...' : texto;
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
          <Image source={equipe.imagem} style={styles.imag} />
          <View style={styles.textos}>
            <Text style={styles.textolistatitulo}>{equipe.titulo}</Text>
            <Text style={styles.textolistacargo}>{equipe.cargo}</Text>
          </View>
        </View>

        <Text style={styles.titulo2}>Tarefas</Text>

        <View style={styles.areabotao}>
          <TouchableOpacity
            style={[styles.botao, { backgroundColor: tarefaspendentes ? '#1A5CFF' : theme.inputBackground }]}
            onPress={botaopendente}
          >
            <Text style={[styles.textobotao, { color: tarefaspendentes ? theme.text4 : theme.text }]}>Pendente</Text>
          </TouchableOpacity>

          <TouchableOpacity
            style={[styles.botao, { backgroundColor: tarefasatrasadas ? '#1A5CFF' : theme.inputBackground }]}
            onPress={botaoatrasado}
          >
            <Text style={[styles.textobotao, { color: tarefasatrasadas ? theme.text4 : theme.text }]}>Atrasados</Text>
          </TouchableOpacity>

          <TouchableOpacity
            style={[styles.botao, { backgroundColor: tarefascompletados ? '#1A5CFF' : theme.inputBackground }]}
            onPress={botaocompletado}
          >
            <Text style={[styles.textobotao, { color: tarefascompletados ? theme.text4 : theme.text }]}>Completados</Text>
          </TouchableOpacity>
        </View>

        <TextInput
          style={styles.navinput}
          placeholder="🔍 Pesquisa uma tarefa"
          placeholderTextColor="#ffffff"
        />

        {tarefaspendentes && renderTarefas(item => item.pendente)}
        {tarefasatrasadas && renderTarefas(item => item.atrasado)}
        {tarefascompletados && renderTarefas(item => item.completado)}

      </ScrollView>
    </View>
  );
}