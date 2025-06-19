import React, { useState } from 'react';
import {Text, TextInput, View, TouchableOpacity, Alert} from 'react-native';
import { styles } from './style';

export default function Login({navigation}){
    const [funcionario, setFuncionario] = useState([
      {
        id: '1',
        nome: 'Gabriel Kenzon Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'Kenzo',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, Jardim Mato Grosso, Cananeia", //mudar isso depois, ver banco de dados
        cargo: "Desenvolvedor Web",
        senha: '1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
      },
      {
        id: '2',
        nome: 'Gabriel Kenzon Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'KenzoAdm',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, Jardim Mato Grosso, Cananeia", //mudar isso depois, ver banco de dados
        cargo: "Desenvolvedor Web",
        senha: '1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
      },
      ]);

    const [email, setEmail] = useState('');
    const [senha, setSenha] = useState('');
    const usuario = funcionario[0];
    const [isSelected, setSelection] = useState(false);

    const verificacao = () => {
      if (email.trim() && senha.trim()) {
        const usuarioLogado = funcionario.find(
          (item) => item.email === email && item.senha === senha
        );

        if (usuarioLogado) {
          if (usuarioLogado.email.includes("Adm")) {
            navigation.navigate('HomeAdm', { usuario: usuarioLogado } );
          } else {
            navigation.navigate('Home', { usuario: usuarioLogado });
          }
        } else {
          Alert.alert('Erro', 'Email ou senha incorretos.');
        }
      } else {
        Alert.alert('Atenção', 'Preencha todos os campos.');
      }
    }
  
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
            onPress={verificacao}
          >
            <Text style={styles.textoBotao}>Login</Text>
          </TouchableOpacity>
        </View>
      );
}
