import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';

export default function Home({navigation}){

    return(

        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <View style={styles.areaperfil}>
                    <Image 
                    style={styles.foto}
                    source={require('../../../assets/img/fotoexemplo.png')} >
                    </Image>
                    <View style={styles.verde}></View>

                    <View style={styles.textoperfil}>
                        <Text style={styles.nome}>Ramon Trigon</Text>
                        <Text style={styles.profissao}>Professor</Text>
                    </View>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>Home</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../assets/img/equipes.png')}
                            style={styles.imagemcard} />
                        <Card.Content
                            onPress={()=> navigation.navigate('Equipes')}
                            style={styles.cardinferior}
                        >
                            <Title style={styles.titulocard}>Equipes</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>

                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../assets/img/tarefas.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content
                            onPress={()=> navigation.navigate('Tarefas')} 
                            style={styles.cardinferior}
                        >
                            <Title style={styles.titulocard}>Tarefas</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>

                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../assets/img/raking.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content
                            onPress={()=> navigation.navigate('Raking')} 
                            style={styles.cardinferior}
                        >
                            <Title style={styles.titulocard}>Raking</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
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
    backgroundColor: "#ffffff"
  },
  scroll: {
    flex: 1,
    width: '100%',
  },
  areaperfil:{
    flexDirection: 'row',
    marginTop: 15,
    width: '100%',
    paddingHorizontal: 20,
  },
  foto:{
    width: 120,
    height: 120,
    marginLeft: 18,
    borderRadius: "100%"
  },
  verde:{
    width:20,
    height:20,
    borderRadius:"100%",
    borderWidth: 3.5,
    borderColor: '#F5F9F9',
    backgroundColor:"#2EBA4E",
    right:28,
    top:95,
  },
  textoperfil: {
    justifyContent: 'center',
    marginLeft: 20, 
  },
  nome: {
    color: '#000',
    fontSize: 16,
    marginBottom: 2,
  },
  profissao: {
    color: '#aaaaaa',
    fontSize: 16,
  },
  areatitulo:{
    justifyContent: 'center',
    marginRight: 50,
    marginTop: 20,
    marginBottom: 30,
  },
  titulo:{
    fontSize: 24,
    marginBottom: 4,
  },
  subtitulo:{
    fontSize: 20,
  },
  areacard:{
    marginBottom: 25,
  },
  cardtarequi:{
    marginBottom: 25,
    width:275,
  },
  imagemcard:{
    height:110,
  },
  cardinferior:{
    backgroundColor: "#ffffffff",
    borderRadius:8,
    borderBottomWidth: -0.1,
    borderBottomColor: '#000',
  },
  titulocard:{
    color:"#000",
    fontWeight: 'bold',
    marginBottom: -5,
  },
  paragraph:{
    color:"#000",
    fontSize: 11,
  },
  linhainfer:{
    flexDirection: 'row',
    marginTop: 11,
    justifyContent: 'space-between',
    width: '100%',
    paddingHorizontal: -10,
    marginBottom: -10,
  },
  data:{
    fontSize: 11,
    color: '#aaaaaa',
  },
  Entre:{
    color:"#000",
    fontSize: 11,
    fontWeight: 'bold',
  },
})