import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, ScrollView} from 'react-native';

export default function TarefasPedentes({navigation}){
    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                  <View style={styles.nav}>
                      <Image
                        style={styles.imagemPerfil}
                      ></Image>
                      <Text style={styles.titulo}>WORKFLOW</Text>
                      <Image
                        style={styles.configuracoes}
                      >
                      </Image>
                  </View>
                <Text style={styles.subtitulo}>Tarefas</Text>
                <View style={styles.areabotao}>
                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasPedentes')}
                  >
                    <Text style={styles.textopedente}>Pedente</Text>
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
                <TouchableOpacity style={styles.areatarefa}>
                  <View style={styles.areatitulo}>
                    <Image
                      style={styles.imagemtarefa}
                    ></Image>
                    <Text>Tarefa 0</Text>
                    <Text>Loremijadsjknadnsakjdnasdnkjldsa</Text>
                  </View>
                  <View style={styles.areaespicificacaodecargo}>
                    <Text style={styles.textocargo}>Lorem</Text>
                    <Text style={styles.data}>31/13/89</Text>
                  </View>
                </TouchableOpacity>
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
  textopedente: {
    color: '#123131'
  } 
})