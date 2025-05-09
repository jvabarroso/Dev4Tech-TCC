import React, { useState } from 'react';
import {  StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';

export default function Equipes({navigation}){
  const [equipe, setEquipe] = useState([
    {
      id: '1',
      titulo: 'Equipe 1',
      cargo: 'Desenvolvimento de Software',
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '2',
      titulo: 'Equipe 2',
      cargo: 'Design',
      imagem: require('../../../assets/img/image.png'),
    }
  ]);

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.titulo}>Equipes</Text>
                <View style={styles.nav}>
                  <TextInput                    
                    style={styles.navinput}
                    placeholder='🔍Pesquisa uma equipe'
                    placeholderTextColor="#ffffff"
                  >
                  </TextInput>
                </View>

                <FlatList
                  data={equipe}
                  keyExtractor={(item) => item.id}
                  style ={styles.flat}
                  renderItem={({ item }) => (
                    <View style={styles.containertarefas}>
                      <Image source={item.imagem} style={styles.imag} />
                        <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                        <Text style={styles.textolistacargo}>{item.cargo}</Text>
                    </View>
                  )}
                />
                
            </View> 
        </ScrollView>
    )
  }



const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    backgroundColor: "#ffffff",
    height: 10000
  },
  titulo: {
    fontSize: 30,
    color: '#000',
    fontWeight: 'bold',
    alignSelf: 'flex-start',
    marginTop:'5%',
    marginBottom: "10%",
    marginLeft: '7%'
  },
  navinput:{
    width: 320,
    padding: 10,
    fontSize: 17,
    backgroundColor: '#1C58F2',
    borderRadius: 10,
    borderBottomWidth: 0.1,
    borderBottomColor: '#000',
    marginBottom: 15,
  },
  flat:{
    marginBottom: 300,
    marginTop:-20,
  },
  containertarefas:{
    height: 25,
    width: 300,
    margin: 40,
    marginRight: 110,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 10,
  },
  imag:{
    width: 45,
    height: 45,
    right:100,
    top: 50
  },
  textolistatitulo:{
    color:'#000',
    fontSize: 20,
    fontWeight: 'bold',
    right:30,
  },
  textolistacargo:{
    color:'#000',
    fontSize: 15,
    textAlign: 'left',
  },
    linhaInfo: {
    flexDirection: 'row',
    marginTop: 15,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
  },


})