import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, CheckBox} from 'react-native';

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
              placeholder="Entre com seu endereço de Email"
              onChangeText={email => setEmail(email)}
            />
            <Text style={styles.texto}>Senha</Text>
            <TextInput
              style={styles.input}
              placeholder="Digite sua senha"
              secureTextEntry={true}
              onChangeText={senha => setSenha(senha)}
            />
            <View style={styles.checkBoxContainer}>
              <CheckBox
                value={isSelected}
                onValueChange={setSelection}
              />
              <Text style={styles.texto}>Lembrar de Mim</Text>
            </View>
            <Text style={styles.senha}>Esqueceu sua senha?</Text>
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
        fontWeight: 'bold',
        color: '#000',
        alignSelf: 'flex-start',
        marginBottom: "30%",
        marginLeft: "2%",
      },
      titulo: {
        fontSize: 35,
        fontWeight: 'bold',
        color: '#000',
        alignSelf: 'flex-start',
        marginTop:'10%',
        marginBottom: "10%",
      },
      area: {
        width: '100%',
        alignItems: 'flex-start',
        marginBottom: "10%",
      },
      texto: {
        fontSize: 18,
        color: '#999999',
        marginBottom: 5,
      },
      input: {
        width: '100%',
        color: '#000842',
        padding: 10,
        fontSize: 16,
        borderRadius: 10,
        borderBottomWidth: 0.1,
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
        color: '#FFFFFF',
      },
})