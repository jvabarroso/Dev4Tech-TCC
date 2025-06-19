import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { styles } from './style';

export default function Home({navigation, route}){

  const usuarioRecebido = route.params?.usuario || {};

    const usuarioPadrao = {
        nome: 'Usuário não identificado',
        cargo: 'Cargo não definido',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    };

    const usuario = {
        ...usuarioPadrao,
        ...usuarioRecebido
    };
    //resolver isso

    return(
        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <View style={styles.areaperfil}>
                    <Image 
                    style={styles.foto}
                    source={require('../../../../assets/img/fotoexemplo.png')} >
                    </Image>
                    <View style={styles.verde}></View>

                    <View style={styles.textoperfil}>
                        <Text style={styles.nome}>Kenzo</Text>
                        <Text style={styles.profissao}>Desenvolvedor Mobile</Text>
                    </View>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>Home</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <TouchableOpacity
                        onPress={()=> navigation.navigate('Equipes')} 
                    >
                        <Card style={styles.cardtarequi}>
                            <Card.Cover 
                                source={require('../../../../assets/img/equipes.png')}
                                style={styles.imagemcard} />
                            <Card.Content style={styles.cardinferior}>
                                <Title style={styles.titulocard}>Equipes</Title>
                                <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                                <View style={styles.linhainfer}>
                                    <Text style={styles.data}>16/07/20</Text>
                                    <Text style={styles.Entre}>Entre aqui</Text>
                                </View>
                            </Card.Content>
                        </Card>
                    </TouchableOpacity>
                    
                    <TouchableOpacity
                         onPress={()=> navigation.navigate('Tarefas')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/tarefas.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Tarefas</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                        onPress={()=> navigation.navigate('Ranking')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/ranking.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Ranking</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>
                    
                </View>
            </View>
        </ScrollView>
    )
}
