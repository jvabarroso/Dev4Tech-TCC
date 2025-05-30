import React from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image} from 'react-native';
import { styles } from './style';

export default function Inicio({navigation}){

    return(
        <View style={styles.container}>
            <Text style={styles.logo}>WORKFLOW</Text>

            <View style={styles.areaTitulo}>
              <Text style={styles.titulo}>Bem vindo ao WORKFLOW </Text>
              <Text style={styles.subtitulo}> Otimize seu Trabalho Conosco! </Text>
            </View>

            <TouchableOpacity
                style={styles.botao}
                onPress={()=> navigation.navigate('Login')}
            >
                <Text style={styles.textoBotao}> Login </Text>
            </TouchableOpacity>
        </View>
    )
}