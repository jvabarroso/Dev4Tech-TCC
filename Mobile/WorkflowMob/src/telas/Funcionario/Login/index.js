import React, { useState } from 'react';
import {Text, TextInput, View, TouchableOpacity, Alert} from 'react-native';
import { showMessage } from "react-native-flash-message";
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

import api from '../../../../services/api';

export default function Login({navigation}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');

    const verificacao = async () => {
      if (!email.trim() || !senha.trim()) {
        showMessage({
          message: 'Atenção Preencha todos os campos.',
          description: 'Preencha as credenciais',
          floating: true,
          statusBarHeight: 70,
          type: "warning",
          duration: 2000,             
        });
        return;
      }
      try {
        const response = await api.post('dev4tec/login.php', {
          Email: email,
          Senha: senha
        }, {
          headers: { 'Content-Type': 'application/json' }
        });
        
        const json = response.data;
        console.log('Dados recebido:', json);

        if (json.success) {
          if (json.role === 'administrador') {
            navigation.navigate('HomeAdm', { usuario: json.usuario});
          } else if (json.role === 'funcionario') {
            navigation.navigate('Home', { usuario: json.usuario});
          }
        } else {
            showMessage({
                message: "Erro",
                description: json.message || 'Credenciais inválidas',
                floating: true,
                statusBarHeight: 70,
                type: "danger",
                duration: 2000,             
            });
        }
      } catch (error) {
          showMessage({
            message: "Erro",
            description: error.response?.data?.message || 'Não foi possível conectar ao servidor',
            floating: true,
            statusBarHeight: 70,
            type: "danger",
            duration: 2000,             
          });
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
