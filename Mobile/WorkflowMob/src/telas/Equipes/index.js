import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, FlatList, TextInput } from 'react-native';
import { styles } from './style';

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
