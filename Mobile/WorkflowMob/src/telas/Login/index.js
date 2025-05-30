import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image} from 'react-native';
import fonts from "../../styles/fonts";

export default function Login({navigation}){

    const [email,setEmail] = useState('');
    const [senha,setSenha] = useState('');
    const [isSelected, setSelection] = useState(false);

    return (
        <View style={styles.container}>
          <Text style={styles.logo}>WORKFLOW</Text>
          <Text style={styles.titulo}>Login</Text>
      
          <View style={styles.area}>
            <Text style={styles.texto}>Email</Text>
            <TextInput
              style={styles.input}
              placeholder="✉️Entre com seu endereço de Email"
              placeholderTextColor={"#000842"}
              onChangeText={email => setEmail(email)}
            />
            <Text style={styles.texto}>Senha</Text>
            <TextInput
              style={styles.input}
              placeholder="🔒Digite sua senha"
              placeholderTextColor={"#000842"}
              secureTextEntry={true}
              onChangeText={senha => setSenha(senha)}
            />
          </View>
      
          <TouchableOpacity
            style={styles.botao}
            onPress={() => navigation.navigate('Home')}
          >
            <Text style={styles.textoBotao}>Login</Text>
          </TouchableOpacity>
        </View>
      );
}


const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff',
        padding: 20,
        alignItems: 'center',
        justifyContent: 'center',
      },
      logo: {
        fontSize: 19,
        fontFamily: fonts.text,
        fontWeight: 'bold',
        color: '#000',
        alignSelf: 'flex-start',
        marginBottom: "28%",
        marginLeft: "2%",
      },
      titulo: {
        fontSize: 35,
        fontWeight: 'bold',
        fontFamily: fonts.text,
        color: '#000',
        alignSelf: 'flex-start',
        marginBottom: "10%",
        marginLeft:10,
        marginTop:40,
      },
      area: {
        width: '100%',
        alignItems: 'flex-start',
        marginBottom: "10%",
        marginLeft:20,
      },
      texto: {
        fontSize: 18,
        fontFamily: fonts.text,
        color: '#999999',
        marginBottom: 5,
      },
      input: {
        width: '100%',
        color: '#000842',
        padding: 10,
        fontSize: 16,
        borderRadius: 10,
        borderBottomWidth: 1.4,
        borderBottomColor: '#000',
        backgroundColor: 'transparet',
        marginBottom: 15,
        
      },
      checkBoxContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        marginBottom: 10,
      },
      senha: {
        color: '#0000FF',
        marginBottom: 20,
      },
      botao: {
        height: 45,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#0C21C1',
        borderRadius: 150,
        width: '75%',
      },
      textoBotao: {
        fontSize: 15,
        fontFamily: fonts.text,
        color: '#FFFFFF',
      },
})