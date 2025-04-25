import React from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image} from 'react-native';

export default function Inicio({navigation}){

    return(
        <View style={styles.container}>
            <Text style={styles.logo}>WORKFLOW</Text>
            <Text style={styles.Titulo}> Bem vindo ao WORKFLOW</Text>
            <Text style={styles.Subtitulo}> Otimize seu Trabalho Conosco! </Text>
            <TouchableOpacity
                style={styles.botao}
                onPress={()=> navigation.navigate('Login')}
            >
                <Text style={styles.textoBotao}> Login </Text>
            </TouchableOpacity>
          
        </View>
    )
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  texto: {
    fontfamily: 'Arial',
    fontSize: 40,
    color: '#f30'
  }
})