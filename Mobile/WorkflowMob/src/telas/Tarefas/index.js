import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';
import {Ionicons} from '@expo/vector-icons';


export default function Tarefas({navigation}){
  const [tarefas, setTarefas] = useState([
    {
      id: '1',
      titulo: 'Tarefa 1',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Desenvolvimento de Software',
      datadeentrega: '16/02/2025',
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '2',
      titulo: 'Tarefa 2',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Documentação',
      datadeentrega: '20/02/2025',
      imagem: require('../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Tarefa 3',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Design',
      datadeentrega: '20/02/2025',
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

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.titulo}>Tarefas</Text>
                <View style={styles.areabotao}>
                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasPedentes')}
                  >
                    <Text style={styles.textobotao}>Pendente</Text>
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasAtrasados')}
                  >
                    <Text style={styles.textobotao}>Atrasados</Text>
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasCompletadas')}
                  >
                    <Text style={styles.textobotao}>Completados</Text>
                  </TouchableOpacity>
                </View>

                <View style={styles.nav}>
                  <TextInput                    
                    style={styles.navinput}
                    placeholder='🔍Pesquisa uma tarefa'
                    placeholderTextColor="#ffffff"
                  >
                  </TextInput>
                </View>

                <FlatList
                  data={tarefas}
                  keyExtractor={(item) => item.id}
                  style={styles.flat}
                  renderItem={({ item }) => (
                    <View style={styles.containertarefas}>

                      <Image source={item.imagem} style={styles.imag} />
                        <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                        <Text style={styles.textolista}>{item.descricao}</Text>
                        <View style={styles.linhaInfo}>

                          <Text style={styles.textolistacargo}>{item.cargo}</Text>
                          <Text style={styles.textolistadata}>{item.datadeentrega}</Text>
                        </View>
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
  areabotao:{
    marginBottom: '11%',
    flexDirection: 'row',
    justifyContent: 'center',
    alignItems: 'center'
  },
  botao:{    
    backgroundColor: '#1C58F2',
    width: '26%',
    height: 25,
    flexDirection: 'row',
    paddingVertical: 10,
    paddingHorizontal: 20,
    marginHorizontal: 6,
    borderRadius: 15,    
    alignItems: 'center',
    alignSelf: 'center',
    justifyContent: 'center'
  },
  textobotao:{
    fontSize: 15,
    color: '#FFFFFF',
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
  },
  containertarefas:{
    height: 80,
    width: 300,
    margin: 40,
    marginRight: 80,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 10,
  },
  linhaInfo: {
    flexDirection: 'row',
    marginTop: 15,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
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
  textolista:{
    color:'#000',
    fontSize: 15,
    left:5
  },
  textolistacargo:{
    color:'#181A1F',
    fontSize: 13,
    backgroundColor: '#F5F7FC',
    borderRadius: 15,
    paddingHorizontal: 7,
    paddingVertical: 3,
  },
  
  textolistadata:{
    color:'#181A1F',
    fontSize: 14,    
  },
})