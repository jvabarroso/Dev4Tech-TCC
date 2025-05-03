import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';

export default function Home({navigation}){

    return(

        <ScrollView style={styles.scroll}>
            <View style={styles.container}>
                <View style={styles.areaperfil}>
                    <Image 
                    style={styles.foto}>
                    </Image>
                    <Text style={styles.nome}>Ramon Trigon</Text>
                    <Text style={styles.profissao}>Professor</Text>
                </View>

                <View style={styles.areatitulo}>
                    <Text style={styles.titulo}>HOME</Text>
                    <Text style={styles.subtitulo}>Explore as Ferramentas</Text>
                </View>

                <View style={styles.areacard}>
                    <Card style={styles.cardtarequi}>
                        <Card.Cover source={{ uri: 'https://via.placeholder.com/150' }} />
                        <Card.Content
                            onPress={()=> navigation.navigate('Equipes')} 
                        >
                            <Title>Equipes</Title>
                            <Paragraph>Loremijadsjknadnsakjdnasdnkjldsa</Paragraph>
                        </Card.Content>
                    </Card>

                    <Card style={styles.cardtarequi}>
                        <Card.Cover source={{ uri: 'https://via.placeholder.com/150' }} />
                        <Card.Content
                            onPress={()=> navigation.navigate('TarefasPedentes')} 
                        >
                            <Title>Tarefas</Title>
                            <Paragraph>daugduiahdiuahsudh</Paragraph>
                        </Card.Content>
                    </Card>

                    <Card style={styles.cardraking}>
                        <Card.Cover source={{ uri: 'https://via.placeholder.com/150' }} />
                        <Card.Content
                            onPress={()=> navigation.navigate('Raking')} 
                        >
                            <Title>Tela de Raking</Title>
                            <Paragraph>sdsdfsfsdffsafds.</Paragraph>
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
  },
  scroll: {
    flex: 1,
    width: '100%',
  }

})