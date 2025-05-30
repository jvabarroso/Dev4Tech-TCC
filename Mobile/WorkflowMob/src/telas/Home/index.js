import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { styles } from './style';

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
