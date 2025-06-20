import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

export default function HomeAdm({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

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
                        <Text style={styles.nome}>KenzoAdm</Text>
                        <Text style={styles.profissao}>Gestor Mobile</Text>
                    </View>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>Home</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <TouchableOpacity
                        onPress={()=> navigation.navigate('CadastroFuncionario')} 
                    >
                        <Card style={styles.cardtarequi}>
                            <Card.Cover 
                                source={require('../../../../assets/img/cadastrarfuncionario.png')}
                                style={styles.imagemcard} />
                            <Card.Content style={styles.cardinferior}>
                                <Title style={styles.titulocard}>Cadastrar Funcionário</Title>
                                <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                                <View style={styles.linhainfer}>
                                    <Text style={styles.data}>16/07/20</Text>
                                    <Text style={styles.Entre}>Entre aqui</Text>
                                </View>
                            </Card.Content>
                        </Card>
                    </TouchableOpacity>
                    
                    <TouchableOpacity
                         onPress={()=> navigation.navigate('CadastroEquipes')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/cadastrarequipes.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Cadastrar Equipes</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                         onPress={()=> navigation.navigate('CadastroTarefas')} 
                    >
                    <Card style={styles.cardtarequi}>
                        <Card.Cover 
                            source={require('../../../../assets/img/tarefas.png')} 
                            style={styles.imagemcard}/>
                        <Card.Content style={styles.cardinferior}>
                            <Title style={styles.titulocard}>Cadastrar Tarefas</Title>
                            <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                            <View style={styles.linhainfer}>
                                <Text style={styles.data}>16/07/20</Text>
                                <Text style={styles.Entre}>Entre aqui</Text>
                            </View>
                        </Card.Content>
                    </Card>
                    </TouchableOpacity>

                    <TouchableOpacity
                        onPress={()=> navigation.navigate('RankingAdm')} 
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
