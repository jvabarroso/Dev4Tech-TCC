import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, ScrollView} from 'react-native';

export default function TarefasAtrasados({navigation}){
    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <Text style={styles.subtitulo}>Tarefas</Text>
                <View style={styles.areabotao}>
                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasPedentes')}
                  >
                    <Text style={styles.textobotao}>Pedente</Text>
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasAtrasados')}
                  >
                    <Text style={styles.textoatrasado}>Atrasados</Text>
                  </TouchableOpacity>

                  <TouchableOpacity
                    style={styles.botao}
                    onPress={()=> navigation.navigate('TarefasCompletadas')}
                  >
                    <Text style={styles.textobotao}>Completadas</Text>
                  </TouchableOpacity>
                </View>
                <View style={styles.areatarefa}>
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
                </View>
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
  textoatrasado: {
    color: '#123131'
  } 
})