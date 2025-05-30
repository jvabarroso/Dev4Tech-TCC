import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, FlatList, TextInput } from 'react-native';
import { styles } from './style';

export default function Tarefas({ navigation }) {
  const [tarefas, setTarefas] = useState([
    {
      id: '1',
      titulo: 'Tarefa 1',
      descricao: 'lorem ipsum kwkkwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww',
      cargo: 'Desenvolvimento de Software',
      datadeentrega: '16/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      imagem: require('../../../assets/img/fotoexemplo.png'),
    },
    {
      id: '2',
      titulo: 'Tarefa 2',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Documentação',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: true,
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Tarefa 3',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Design',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '4',
      titulo: 'Tarefa 4',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Back-end',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      selproblema: false,
      imagem: require('../../../assets/img/image.png'),
    },
  ]);

  function limitarTexto(texto, limite) {
    return texto.length > limite ? texto.substring(0, limite) + '...' : texto;
}

  return (
    <View style={styles.container}>
      <FlatList
        data={tarefas}
        keyExtractor={(item) => item.id}
        style={styles.flat}
        ListHeaderComponent={() => (
          <View>
            <Text style={styles.titulo}>Tarefas</Text>

            <View style={styles.areabotao}>
              <TouchableOpacity
                style={styles.botao}
                onPress={() => navigation.navigate('TarefasPedentes')}
              >
                <Text style={styles.textobotao}>Pendente</Text>
              </TouchableOpacity>

              <TouchableOpacity
                style={styles.botao}
                onPress={() => navigation.navigate('TarefasAtrasados')}
              >
                <Text style={styles.textobotao}>Atrasados</Text>
              </TouchableOpacity>

              <TouchableOpacity
                style={styles.botao}
                onPress={() => navigation.navigate('TarefasCompletadas')}
              >
                <Text style={styles.textobotao}>Completados</Text>
              </TouchableOpacity>
            </View>

            <TextInput
              style={styles.navinput}
              placeholder="🔍 Pesquisa uma tarefa"
              placeholderTextColor="#ffffff"
            />
          </View>
        )}
        renderItem={({ item }) => (
          <TouchableOpacity
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
        )}
        contentContainerStyle={{ paddingBottom: 100 }}
      />
    </View>
  );
}