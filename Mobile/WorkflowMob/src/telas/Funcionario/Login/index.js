import React, { useState } from 'react';
import {Text, TextInput, View, TouchableOpacity, Alert} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import api from '../../../../services/api';

export default function Login({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');

    const verificacao = async () => {
      if (email.trim() && senha.trim()) {
        try{
          console.log('Dados enviados:', { Email: email, Senha: senha });
          const response = await api.post('dev4tec/login.php', {
            Email: email,
            Senha: senha
          });
          console.log('Resposta da API:', response.data);

          const json = response.data;
          
          if (json.success) {
            if (json.role === 'administrador') {
              navigation.navigate('HomeAdm', { usuario: json.usuario } );
            } else if (json.role === 'funcionario') {
              navigation.navigate('Home', { usuario: json.usuario });
            } else {
            Alert.alert('Erro', 'Email ou senha incorretos.');
            }
          }else {
            Alert.alert('Atenção', 'Preencha todos os campos.');
            } 
        }catch (error) {
          Alert.alert('Erro', 'Não foi possível conectar ao servidor.');
          console.error(error);
        }
        } else {
            Alert.alert('Atenção', 'Preencha todos os campos.');
      }
    };

    return (
        <View style={styles.container}>
          <Text style={styles.logo}>WORKFLOW</Text>
          <Text style={styles.titulo}>Login</Text>

          <View style={styles.area}>
            <Text style={styles.texto}>Email</Text>
            <TextInput
              style={styles.input}
              placeholder="✉️Entre com seu endereço de Email"
              placeholderTextColor={theme.text}
              onChangeText={email => setEmail(email)}
            />
            <Text style={styles.texto}>Senha</Text>
            <TextInput
              style={styles.input}
              placeholder="🔒Digite sua senha"
              placeholderTextColor={theme.text}
              secureTextEntry={true}
              onChangeText={senha => setSenha(senha)}
            />
          </View>
      
          <TouchableOpacity
            style={styles.botao}
            onPress={verificacao}
          >
            <Text style={styles.textoBotao}>Login</Text>
          </TouchableOpacity>
        </View>
      );
}
