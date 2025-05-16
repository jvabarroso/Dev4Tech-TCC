import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';
import {Ionicons} from '@expo/vector-icons';


export default function TarefaEnvio({ route }){
    const { tarefas } = route.params;

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>   
                <View style={styles.nav}>
                    <TouchableOpacity
                        style={styles.botaodevoltar}
                        onPress={()=> navigation.navigate('Tarefas')}
                    ><Ionicons name="arrow-back" size={24} color="black" /></TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                </View>

                <Image source={tarefas.imagem} style={styles.imagem} />
                <Text style={styles.titulotarefa}>{tarefas.titulo}</Text>
                <Text style={styles.datadeenvio}>Postado em {tarefas.datadeenvio}</Text>
                <View style={styles.areadetalhes}>
                    <View style={styles.linha}>
                        <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                        <Text style={styles.subtitulos2}>EQUIPE</Text>
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
                            <Text style={styles.textoadd}>Anexar um arquivo</Text>
                        </TouchableOpacity>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textoadd}>Adicionar uma mensagem</Text>
                        </TouchableOpacity>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textoproblem}>Relatar problema</Text>
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
  nav:{
    flexDirection: 'row',
    marginTop: 20,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
  },
  botaodevoltar:{
    width:10,
    height:10,
  },
  botao:{
    color: "#000000"
  },
  titulo:{
    fontSize: 18,
    marginRight: 110
  },
  imagem:{
    width:80,
    height:80,
    marginRight:220,
    marginTop:40,
    borderRadius: 8,
  },
  titulotarefa:{
    fontSize:25,
    fontWeight: 'bold',
    color:"#000000",
    marginTop:10,
    marginRight:200,
  },
  datadeenvio:{
    fontSize:13,
    marginTop:8,
    marginRight:130,
    color:"#aaaaaa",
  },
  linha:{
    flexDirection: 'row',
    marginTop: 10,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 20,
  },
  subtitulos:{
    fontSize:13,
    marginTop:8,
    color:"#181A1F",
    fontWeight: 'bold',
  },

  

})