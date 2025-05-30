import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, FlatList, TextInput } from 'react-native';
import fonts from "../../styles/fonts";

export default function Equipes({ navigation }) {
  const [equipe, setEquipe] = useState([
    {
      id: '1',
      titulo: 'Equipe 1',
      cargo: 'Desenvolvimento de Software',
      tarefaspostadas: 20,
      quantdeproblemas:6,
      tarefasatrasadas:1,
      tarefasnaoentregues: 6,
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '2',
      titulo: 'Equipe 2',
      cargo: 'Design',
      tarefaspostadas: 10,
      quantdeproblemas:2,
      tarefasatrasadas:2,
      tarefasnaoentregues: 2,
      imagem: require('../../../assets/img/image.png'),
    }
  ]);

  return (
    <View style={styles.container}>
      <FlatList
        data={equipe}
        keyExtractor={(item) => item.id}
        style={styles.flat}
        ListHeaderComponent={() => (
          <View>
            <Text style={styles.titulo}>Equipes</Text>

            <TextInput
              style={styles.navinput}
              placeholder="🔍 Pesquisa uma equipe"
              placeholderTextColor="#ffffff"
            />
          </View>
        )}
        
        renderItem={({ item }) => (
          <View style={styles.containertarefas}>
            <Image source={item.imagem} style={styles.imag} />
            <View style={styles.textos}>
              <Text style={styles.textolistatitulo}>{item.titulo}</Text>
              <Text style={styles.textolistacargo}>{item.cargo}</Text>
            </View>
          </View>
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
    marginBottom: "8%",
    fontFamily: fonts.text,
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
    padding: 10,
    marginBottom: 20,
    flexDirection: 'row',
    alignItems: 'center',
  },
  imag: {
    width: 45,
    height: 45,
    marginLeft: 10,
  },
  textos: {
    marginLeft: 15,
    flex: 1,
  },
  textolistatitulo: {
    color: '#000',
    fontSize: 18,
    fontWeight: 'bold',
    fontFamily: fonts.text,
  },
  textolistacargo: {
    color: '#000',
    fontSize: 15,
    fontFamily: fonts.text,
  },
});
