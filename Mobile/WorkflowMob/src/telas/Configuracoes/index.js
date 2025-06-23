import React, { useState } from 'react';
import { Text, TextInput, View, TouchableOpacity, Image, ScrollView, useColorScheme} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../styles/themecontext'
import {Ionicons} from '@expo/vector-icons';

export default function Configuracoes({navigation, route}){
    const { theme, toggleTheme } = useTheme();
    const styles = getStyles(theme);

    const usuario = route.params?.usuario || {
      nome: 'Usuário não identificado',
      cargo: 'Cargo não definido',
      dataNascimento: "Data não definido",
      email:"Email não definido",
      telefone:'Telefone não definido',
      endereco:'Endereço não definido',
      cpf:"CPF não definido"
    };
    

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

    const [mostrardados, setMostrardados] = useState(false);
    const [mostrarfuncionario, setMostrarfuncionario] = useState(false);

    return(
      <View style={styles.container}>
        <ScrollView 
          contentContainerStyle={styles.scrollContent } keyboardShouldPersistTaps="handled">
          <View style={styles.nav}>
            <Text style={styles.logo}>WORKFLOW</Text>
          </View>
            <Text style={styles.titulo}>Configurações</Text>

            <View style={styles.linha}> 
              <TouchableOpacity
                style={styles.botaodevoltar}
                onPress={()=> navigation.goBack()}
              >
                <Ionicons name="chevron-back-outline" size={18} color="black" style={styles.botaodevoltar}/>
                <Text style={styles.voltar}>Voltar</Text>
              </TouchableOpacity>

              <Text style={styles.pontuacao}>
                Pontuação:<Text style={styles.pontuacaotext}>100</Text>
              </Text>
            </View>

            <View style={styles.containerfuncionario}>
              <Image source={require('../../../assets/img/image.png')} style={styles.imagemfuncionario} />
              <View style={styles.textos}>
                <Text style={styles.textofuncionario}>{usuario.nome}</Text>
                <Text style={styles.textofuncionariocargo}>{usuario.cargo}</Text>
              </View>
            </View>

            <View style={styles.linha}>
              <Text style={styles.titulo2}>Equipes</Text>
              <TouchableOpacity
                style={styles.botaomodo} 
                onPress={toggleTheme}
              >
                <Text style={styles.textomodo}>
                  { theme.mode === 'dark' ? 'Modo Claro' : 'Modo Escuro' }
                </Text>
              </TouchableOpacity>
            </View>

            {mostrarfuncionario && equipe.map((item) => (
              <View key={item.id} style={styles.containertarefas}>
                <Image source={item.imagem} style={styles.imag} />
                <View style={styles.textos}>
                  <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                  <Text style={styles.textolistacargo}>{item.cargo}</Text>
                </View>
              </View>
            ))}
            <TouchableOpacity 
              style={styles.inputfuncionario}
              onPress={() => setMostrarfuncionario(!mostrarfuncionario)}
            >
              <Text style={styles.textobotao3}>{mostrarfuncionario ? 'Ocultar' : 'Ver Equipes'}</Text>
            </TouchableOpacity>

            {mostrardados && (
              <View style={styles.areaInput}>
                <Text style={styles.texto}>Nome</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.nome} //depois mudar, mensagem para mim mesmo dnv :D
                  placeholderTextColor={theme.text}
                />
                <Text style={styles.texto}>Data de nascimento</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.dataNascimento}
                  placeholderTextColor={theme.text}
                  secureTextEntry={true}
                />
                <Text style={styles.texto}>CPF</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.cpf}
                  placeholderTextColor={theme.text}
                  secureTextEntry={true}
                />
                <Text style={styles.texto}>Email</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.email}
                  placeholderTextColor={theme.text}
                  secureTextEntry={true}
                />
                <Text style={styles.texto}>Telefone</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.telefone}
                  placeholderTextColor={theme.text}
                />
                <Text style={styles.texto}>Endereço</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.endereco}
                  placeholderTextColor={theme.text}
                  secureTextEntry={true}
                />
                <Text style={styles.texto}>Cargo</Text>
                <TextInput
                  style={styles.input}
                  placeholder={usuario.cargo}
                  placeholderTextColor={theme.text}
                  secureTextEntry={true}
                />
                <TouchableOpacity 
                  style={styles.botaoeditar}
                >    
                  <Text style={styles.textoeditar}>Editar dados</Text>                     
                </TouchableOpacity>
              </View>
            )}
            <TouchableOpacity 
              style={styles.inputfuncionario}
              onPress={() => setMostrardados(!mostrardados)}
            >
              <Text style={styles.textobotao3}>{mostrardados ? 'Ocultar' : 'Ver Dados pessoais'}</Text>
            </TouchableOpacity>

            <TouchableOpacity 
              style={styles.botaosair}
              onPress={()=> navigation.navigate('Login')} 
            >    
              <Text style={styles.textoeditar}>Sair da conta</Text>                     
            </TouchableOpacity>

        </ScrollView>
      </View>
    )
}
