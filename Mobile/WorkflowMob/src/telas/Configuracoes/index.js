import React, { useState } from 'react';
import { StyleSheet, Text, TextInput, View, TouchableOpacity, Image, ScrollView} from 'react-native';
import { styles } from './style';
import {Ionicons} from '@expo/vector-icons';

export default function Configuracoes({navigation}){
    const [funcionario, setFuncionario] = useState([
      {
        id: '1',
        nome: 'Gabriel Kenzon Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'gabrieltakeuchi417@gmail.com',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, Jardim Mato Grosso, Cananeia", //mudar isso depois, ver banco de dados
        cargo: "Desenvolvedor Web",
        imagem: require('../../../assets/img/fotoexemplo.png'),
      },
      ]);

    const [equipe, setEquipe] = useState([
      {
        id: '1',
        titulo: 'Equipe 1',
        cargo: 'Desenvolvimento de Software',
        tarefaspostadas: 20,
        quantdeproblemas:6,
        tarefasatrasadas:1,
        tarefasnaoentregues: 6,
        imagem: require('../../../assets/img/image.png'),
      },
      {
        id: '2',
        titulo: 'Equipe 2',
        cargo: 'Design',
        tarefaspostadas: 10,
        quantdeproblemas:2,
        tarefasatrasadas:2,
        tarefasnaoentregues: 2,
        imagem: require('../../../assets/img/image.png'),
      },
            {
        id: '3',
        titulo: 'Equipe 1',
        cargo: 'Desenvolvimento de Software',
        tarefaspostadas: 20,
        quantdeproblemas:6,
        tarefasatrasadas:1,
        tarefasnaoentregues: 6,
        imagem: require('../../../assets/img/image.png'),
      },
            {
        id: '4',
        titulo: 'Equipe 1',
        cargo: 'Desenvolvimento de Software',
        tarefaspostadas: 20,
        quantdeproblemas:6,
        tarefasatrasadas:1,
        tarefasnaoentregues: 6,
        imagem: require('../../../assets/img/image.png'),
      },
      
    ]);
    return(
      <View style={styles.container}>
        <ScrollView 
          style={styles.scrollView}
          contentContainerStyle={styles.containerConteudo}>
          <View style={styles.nav}>
            <Text style={styles.logo}>WORKFLOW</Text>
          </View>
            <Text style={styles.titulo}>Configurações</Text>
            <View style={styles.linha}> 
              <TouchableOpacity
                style={styles.botaodevoltar}
                onPress={()=> navigation.navigate('Tarefas')}
              >
                <Ionicons name="chevron-back-outline" size={18} color="black" style={styles.botaodevoltar}/>
                <Text style={styles.voltar}>Voltar</Text>
              </TouchableOpacity>
              <Text style={styles.pontuacao}>Pontuação:<Text style={styles.pontuacaotext}>1111</Text></Text>
            </View>

            <View style={styles.containerfuncionario}>
              <Image source={require('../../../assets/img/image.png')} style={styles.imagemfuncionario} />
              <View style={styles.textos}>
                <Text style={styles.textofuncionario}>Funcionario 1</Text>
                <Text style={styles.textofuncionariocargo}>Professor</Text>
              </View>
            </View>

            <Text style={styles.titulo2}>Equipes</Text>
            {equipe.map((item) => (
              <View key={item.id} style={styles.containertarefas}>
                <Image source={item.imagem} style={styles.imag} />
                <View style={styles.textos}>
                  <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                  <Text style={styles.textolistacargo}>{item.cargo}</Text>
                </View>
              </View>
            ))}

          <View style={styles.areaInput}>
            <Text style={styles.texto}>Nome</Text>
            <TextInput
              style={styles.input}
              placeholder="Gabriel Kenzo" //depois mudar, mensagem para mim mesmo dnv :D
              placeholderTextColor={"#000000"}
            />
            <Text style={styles.texto}>Data de nascimento</Text>
            <TextInput
              style={styles.input}
              placeholder="xx/xx/xxxx"
              placeholderTextColor={"#000000"}
              secureTextEntry={true}
            />
            <Text style={styles.texto}>Email</Text>
            <TextInput
              style={styles.input}
              placeholder="joaovitinhocraft@gmail.com"
              placeholderTextColor={"#000000"}
              secureTextEntry={true}
            />
            <Text style={styles.texto}>Telefone</Text>
            <TextInput
              style={styles.input}
              placeholder="1399899989"
              placeholderTextColor={"#000000"}
            />
            <Text style={styles.texto}>Endereço</Text>
            <TextInput
              style={styles.input}
              placeholder="Rua João da Fonseca, Jardim Mato Grosso, Cananeia senha"
              placeholderTextColor={"#000000"}
              secureTextEntry={true}
            />
            <Text style={styles.texto}>Cargo</Text>
            <TextInput
              style={styles.input}
              placeholder="Analista de Marketing"
              placeholderTextColor={"#000000"}
              secureTextEntry={true}
            />
          </View>
        </ScrollView>
      </View>
    )
}
