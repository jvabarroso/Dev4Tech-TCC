import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, FlatList, TextInput } from 'react-native';
import { Ionicons } from '@expo/vector-icons';

export default function Tarefas({ navigation }) {
  const [tarefas, setTarefas] = useState([
    {
      id: '1',
      titulo: 'Tarefa 1',
      descricao: 'lorem ipsum kwkkwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwwww',
      cargo: 'Desenvolvimento de Software',
      datadeentrega: '16/02/2025',
      datadeenvio: '07/02/2025',
      imagem: require('../../../assets/img/fotoexemplo.png'),
    },
    {
      id: '2',
      titulo: 'Tarefa 2',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Documentação',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Tarefa 3',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Design',
      datadeentrega: '20/02/2025',
      datadeenvio: '07/02/2025',
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '4',
      titulo: 'Tarefa 4',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Back-end',
      datadeentrega: '20/02/2025',
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

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
    paddingHorizontal: 20,
  },
  titulo: {
    fontSize: 30,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "10%",
  },
  areabotao: {
    marginBottom: 20,
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center'
  },
  botao: {
    backgroundColor: '#1C58F2',
    paddingVertical: 10,
    paddingHorizontal: 15,
    marginHorizontal: 6,
    borderRadius: 15,
    alignItems: 'center',
    justifyContent: 'center'
  },
  textobotao: {
    fontSize: 15,
    color: '#FFFFFF',
  },
  navinput: {
    width: '100%',
    padding: 10,
    fontSize: 17,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
    color: '#fff',
  },
  flat: {
    flex: 1,
  },
  containertarefas: {
    backgroundColor: '#F5F7FC',
    borderRadius: 10,
    padding: 15,
    marginBottom: 20,
  },
  linhaTarefa: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 10,
  },
  textosTarefa: {
    marginLeft: 10,
    flexShrink: 1,
  },
  imag: {
    width: 45,
    height: 45,
  },
  textolistatitulo: {
    color: '#000',
    fontSize: 18,
    fontWeight: 'bold',
  },
  textolista: {
    color: '#000',
    fontSize: 15,
  },
  linhaInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 5,
  },
  textolistacargo: {
    color: '#181A1F',
    fontSize: 13,
    backgroundColor: '#DDE3F0',
    borderRadius: 15,
    paddingHorizontal: 7,
    paddingVertical: 3,
  },
  textolistadata: {
    color: '#181A1F',
    fontSize: 14,
  },
});
