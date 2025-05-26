import React, { useState } from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image, ScrollView, FlatList, TextInput} from 'react-native';
import {Ionicons} from '@expo/vector-icons';


export default function TarefaEnvio({ navigation, route }){
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

                  <View style={styles.colunaEquipe}>
                    <Text style={styles.subtitulos}>EQUIPE</Text>
                    <Text style={styles.cargos}>{tarefas.cargo}</Text>
                  </View>
                </View>
                    <View style={styles.linha2}>
                        <Text style={styles.titulodescricao}>DESCRIÇÃO DA TAREFA</Text>
                        <Text style={styles.descricao2}>{tarefas.descricao}</Text>
                        <TouchableOpacity
                            style={styles.botaomostrar}
                        >
                            <Text style={styles.textodescr}>Ver mais</Text>
                        </TouchableOpacity>
                    </View>

                    <View style={styles.linha2}>
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
    paddingVertical:25,
    height:780,
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
  linha2:{
    flexDirection: 'column',
    justifyContent: 'flex-start',
    width: '100%',
    marginTop: 10,
    paddingHorizontal: 5,
    gap: 3,
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
  coluna: {
    width: '48%',
  },
  colunaEquipe: {
    width: '48%',
    alignItems: 'flex-start', 
    gap:7,
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
    marginLeft: -5,

  },
  titulodescricao: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: 4,
},
  descricao2: {
    fontSize: 14,
    color: '#333',
},
  botaomostrar: {
    paddingVertical: 3,
},
  botaoenviar:{
    backgroundColor: '#1C58F2',
    width:100,
    height:40,
    paddingVertical: 10,
    paddingHorizontal: 25,
    borderRadius: 10,
    justifyContent:"center",
    alignContent:"center",
    marginTop:100,
    marginLeft:128,
},
  textoenvio: {
    color: '#fff',
    fontSize: 16,
    fontWeight: 'bold',
},
 textoadd:{
    color: "#1C58F2",
    fontWeight: 'bold', 
 },
 textoproblem:{
    color: "#F21C1C",
    fontWeight: 'bold', 
 },
})