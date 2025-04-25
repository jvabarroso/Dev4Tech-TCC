import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox} from 'react-native';

export default function Login({navigation}){

    const [email,setEmail] = useState('');
    const [senha,setSenha] = useState('');
    const [isSelected, setSelection] = useState(false);


    return(
        <View style={styles.container}>
            <Text style={styles.logo}>WORKFLOW</Text>
            <Text style={styles.Titulo}> Login </Text>

            <View style={styles.Area}>
                <Text style={styles.texto}> Email: </Text>
                <TextInput
                    style={styles.input}
                    placeholder='Entre com seu endereço de Email'
                    onChangeText={email => setEmail(email)}
                ></TextInput>
            </View>

            <View style={styles.Area}>
                <Text style={styles.texto}> Senha: </Text>
                <TextInput
                    style={styles.input}
                    placeholder='Digite sua senha'
                    secureTextEntry={true}
                    onChangeText={senha => setSenha(senha)}
                ></TextInput>
            </View>

            <View style={styles.checkBoxContainer}>
                <CheckBox
                    style={styles.checkBox}
                    onValueChange={isSelected =>setSelection(isSelected)}
                >
                    <Text style={styles.texto}> Lembrar de Mim </Text>
                </CheckBox>
            </View>

            <Text style={styles.senha}> Esqueceu sua senha? </Text>

            <TouchableOpacity
                style={styles.botao}
                onPress={()=> navigation.navigate('Home')} 
            >
                <Text style={styles.textoBotao}> Login </Text>
            </TouchableOpacity>
          
        </View>
    )
}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  texto: {
    fontfamily: 'Arial',
    fontSize: 40,
    color: 'black'
  },
  botao:{
    backgroundColor:'black',
  },
  textoBotao:{
    color:'white'
  }
})