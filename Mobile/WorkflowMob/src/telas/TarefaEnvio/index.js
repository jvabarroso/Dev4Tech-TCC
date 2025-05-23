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
                    ><Ionicons name="arrow-back" size={24} color="black" style={styles.botaodevoltar} /></TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                </View>

                <Image source={tarefas.imagem} style={styles.imagem} />
                <Text style={styles.titulotarefa}>{tarefas.titulo}</Text>
                <Text style={styles.datadeenvio}>Postado em {tarefas.datadeenvio}</Text>
                <View style={styles.areadetalhes}>
               <View style={styles.linha}>
                  <View style={styles.coluna}>
                    <Text style={styles.subtitulos}>PRAZO DE ENTREGA</Text>
                    <Text style={styles.datas}>{tarefas.datadeentrega}</Text>
                  </View>

                  <View style={styles.coluna}>
                    <Text style={[styles.subtitulos, { textAlign: 'right' }]}>EQUIPE</Text>
                    <Text style={[styles.cargos, { alignSelf: 'flex-end' }]}>{tarefas.cargo}</Text>
                  </View>
                </View>
                    <View style={styles.linha}>
                        <Text style={styles.titulodescricao}>DESCRIÇÃO DA TAREFA</Text>
                        <Text style={styles.c}>{tarefas.descricao}</Text>
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
    backgroundColor: "#ffffff",
    paddingHorizontal: 20,
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
    width:30,
    height:30,
    marginLeft: -10,
    marginTop:15,
  
  },
  botao:{
    color: "#000000"
  },
  titulo:{
    fontSize: 18,
    marginRight: 110,
    marginTop:27,
  },
  imagem:{
    width:80,
    height:80,
    marginRight:220,
    marginTop:60,
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
    marginTop:6,
    marginLeft:5,
    color:"#aaaaaa",
  },
  linha:{
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    marginTop: 10,
    paddingHorizontal: 5,
  },
  subtitulos:{
    fontSize:13,
    marginTop:4,
    color:"#181A1F",
    fontWeight: 'bold',
  },
  subtitulos2:{
    fontSize:13,
    marginTop:4,
    paddingRight:14,
    color:"#181A1F",
    fontWeight: 'bold',
  },
  linha2: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: 7,
    marginTop: 5,
  },

  coluna: {
    width: '48%',
  },

  datas: {
    fontSize: 14,
    color: '#000',
  },

  cargos: {
    fontSize: 14,
    color: '#181A1F',
    backgroundColor: '#F5F7FC',
    borderRadius: 20,
    paddingHorizontal: 8,
    paddingVertical: 3,
    marginLeft: -10,

  },

})