import React from 'react';
import { StyleSheet, Text, View, TouchableOpacity, Image} from 'react-native';

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

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
  logo: {
    fontfamily: 'Arial',
    fontSize: 19,
    color: '#000',
    fontWeight: 'bold',
    flexDirection: 'row',
    paddingBottom: 5,
    marginBottom: '135%',
    marginRight: '60%',
  },
  areaTitulo: {
    position: 'absolute',
    alignItems: 'center',
    top: '30%',
  },
  titulo: {
    fontfamily: 'Arial',
    fontSize: 35,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: 10,
    marginRight: 110,
  },
  subtitulo: {
    fontfamily: 'Arial',
    fontSize: 18,
    fontWeight: 'bold',
    color: '#000',
    marginBottom: 10,
    marginRight: 40,
    width:300,
  },
  botao:{
    position: 'absolute',
    top: '63%',
    height: 45,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#0C21C1',
    borderRadius: 150,
    width: '75%',
    alignSelf: 'center',
  },
  textoBotao: {
    fontSize: 15,
    color: '#FFFFFF'
  },
})