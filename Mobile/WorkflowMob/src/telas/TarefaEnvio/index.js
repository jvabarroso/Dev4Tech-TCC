import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';
import {Ionicons} from '@expo/vector-icons';
import { strictEqual } from 'assert';


export default function TarefaEnvio({ route }){
    const { tarefas } = route.params;

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>   
                <View style={styles.nav}>
                    <TouchableOpacity
                        style={styles.botaodevoltar}
                        onPress={()=> navigation.navigate('Tarefas')}
                    ></TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                </View>

                <Image source={tarefas.imagem} style={styles.imagem} />
                <Text style={styles.titulotarefa}>{tarefas.titulo}</Text>
                <Text>Postado em {tarefas.datadeenvio}</Text>
                <View style={styles.areadetalhes}>
                    <View style={styles.linha}>
                        <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                        <Text style={styles.subtitulos}>EQUIPE</Text>
                    </View>
                    <View style={styles.linha}>
                        <Text style={styles.datas}>{tarefas.datadeentrega}</Text>
                        <Text style={styles.cargos}>{tarefas.cargo}</Text>
                    </View>
                    <View style={styles.linha}>
                        <Text style={styles.titulodescricao}>DESCRIÇÃO DA TAREFA</Text>
                        <Text style={styles.cargos}>{tarefas.descricao}</Text>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textodescr}>Ver mais</Text>
                        </TouchableOpacity>
                    </View>

                    <View style={styles.areaenvio}>
                        <Text style={styles.subtitulos}>MEU TRABALHO</Text>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textoadd}>Ver mais</Text>
                        </TouchableOpacity>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textoadd}>Ver mais</Text>
                        </TouchableOpacity>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textoproblem}>Ver mais</Text>
                        </TouchableOpacity>
                    </View>
                    <TouchableOpacity
                        style={styles.botaoenviar}
                    >
                        <Text style={styles.textoenvio}>Enviar</Text>
                    </TouchableOpacity>
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
    backgroundColor: "#ffffff",
    height: 10000
  },
    imagem: {
    width: 100,
    height: 100,
    marginVertical: 20
  },
})