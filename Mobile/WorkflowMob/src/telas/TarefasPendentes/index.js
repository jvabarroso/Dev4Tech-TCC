import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';

export default function TarefasPendentes({navigation}){
  const [tarefas, setTarefas] = useState([
    {
      id: '1',
      titulo: 'Tarefa 1',
      descricao: 'lorem ipsum kwkkww',
      cargo: 'Desenvolvimento de Software',
      datadeentrega: '16/02/2025'
    },
    {
      id: '2',
      titulo: 'Tarefa 2',
      descricao: 'documentar api',
      cargo: 'Documentação',
      datadeentrega: '20/02/2025'
    }
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
                    <Text style={styles.textopedente}>Pendente</Text>
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
                    <Text style={styles.textobotao}>Completadas</Text>
                  </TouchableOpacity>
                </View>
                <View style={styles.nav}>
                  <TextInput
                    style={styles.navinput}
                    placeholder='Pesquisa uma Tarefas'
                  >
                  </TextInput>
                </View>
                <FlatList
                  data={tarefas}
                  keyExtractor={(item) => item.id}
                  renderItem={({ item }) =>    
                  <View style={styles.containertarefas}>
                    <Text style={styles.textolista}>{item.titulo}</Text>
                    <Text style={styles.textolista}>{item.descricao}</Text>
                    <Text style={styles.textolista}>{item.cargo}</Text>
                    <Text style={styles.textolista}>{item.datadeentrega}</Text>
                  </View>}
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
  },
  titulo: {
    fontfamily: 'Arial',
    fontSize: 35,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: '2%',
    marginLeft: '3%',
  },
  botao:{
    position: 'absolute',
    height: 20,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#0C21C1',
    borderRadius: 150,
    width: '25%',
    alignSelf: 'center',
  },
  containertarefas:{
    height: 80,
    margin: 10,
    justifyContent: 'center',
    alignItems: 'center',
    borderRadius: 10
  },
  textolista:{
    color:'#000',
    fontSize: 20,
    fontWeight: 'bold'
  }
})