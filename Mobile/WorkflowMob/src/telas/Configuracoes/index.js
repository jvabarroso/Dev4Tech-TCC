import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, FlatList} from 'react-native';

import {Ionicons} from '@expo/vector-icons';

export default function Configuracoes({navigation}){
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
    return(
      <View style={styles.container}>
        <View style={styles.nav}>
          <Text style={styles.titulo}>WORKFLOW</Text>
        </View>
          <FlatList
            data={equipe}
            keyExtractor={(item) => item.id}
            style={styles.flat}
            ListHeaderComponent={() => (
              <View>
                <Text style={styles.titulo}>Configurações</Text>
                <View style={styles.linha}> 
                  <TouchableOpacity
                    style={styles.botaodevoltar}
                    onPress={()=> navigation.navigate('Tarefas')}
                  ><Ionicons name="arrow-back" size={24} color="black" style={styles.botaodevoltar}/><Text style={styles.voltar}>Voltar</Text></TouchableOpacity>
                  <Text style={styles.pontuacao}>Pontuação:</Text>
                </View>
                <View style={styles.containerfuncionario}>
                  <Image source={require('../../../assets/img/image.png')} style={styles.imagemfuncionario} />
                  <View style={styles.textos}>
                    <Text style={styles.textofuncionario}>Funcionario 1</Text>
                    <Text style={styles.textofuncionariocargo}>Professor</Text>
                  </View>
                </View>

                <Text style={styles.titulo2}>Equipes</Text>
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
    )
}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#ffffff",
    paddingHorizontal: 20,
  },
  nav:{
    flexDirection: 'row',
    marginTop: 20,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
  },
  botaodevoltar:{
    width:30,
    height:30,
    marginLeft: 1,
  },
  titulo: {
    fontSize: 30,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "8%",
  },
  titulo2: {
    fontSize: 15,
    color: '#000',
    fontWeight: 'bold',
    marginTop: '5%',
    marginBottom: "8%",
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
  containerfuncionario: {
    backgroundColor: '#ffffff',
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
  imagemfuncionario: {
    width: 90,
    height: 90,
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
  },
  textolistacargo: {
    color: '#000',
    fontSize: 15,
  },
  textofuncionario: {
    color: '#000',
    fontSize: 28,
    fontWeight: 'bold',
  },
  textofuncionariocargo: {
    color: '#000',
    fontSize: 19,
  },
  pontuacao: {
    fontSize: 16,
    fontWeight: 'bold',
    marginBottom: 10,
  },
  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    marginTop: 10,
    paddingHorizontal: 5,
  },
})