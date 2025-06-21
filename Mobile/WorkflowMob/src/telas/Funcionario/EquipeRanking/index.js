import React, { useState } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';

export default function EquipeRanking({ navigation, route}) {
  const { theme } = useTheme();
  const styles = getStyles(theme);

  const { equipe } = route.params;
  
  const [funcionario, setFuncionario] = useState([
    {
        id: '1',
        nome: 'Gabriel Kenzo Takeuchi',
        datadenascimento: "16/05/1980",
        email: 'kenzo@empresa.com',
        telefone: 13982176670,
        endereco: "Rua João da Fonseca, 123 - Jardim Mato Grosso, Cananeia/SP",
        cargo: "Desenvolvedor Web Sênior",
        equipe: null,
        tarefaspostadas: 30,
        tarefasatrasadas:9,
        tarefasnaoentregues: 10,
        senha: '1234',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '2',
        nome: 'Leonardo Silva',
        datadenascimento: "22/11/1992",
        email: 'leonardo.silva@empresa.com',
        telefone: 11987654321,
        endereco: "Av. Paulista, 1000 - São Paulo/SP",
        cargo: "Designer UX/UI",
        equipe: null,
        tarefaspostadas: 30,
        tarefasatrasadas:9,
        tarefasnaoentregues: 10,
        senha: 'abcd123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '3',
        nome: 'Ana Carolina Oliveira',
        datadenascimento: "05/03/1985",
        email: 'ana.oliveira@empresa.com',
        telefone: 21999887766,
        endereco: "Rua das Flores, 45 - Rio de Janeiro/RJ",
        cargo: "Gerente de Projetos",
        equipe: null,
        tarefaspostadas: 10,
        tarefasatrasadas:2,
        tarefasnaoentregues: 2,
        senha: 'ana2023',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
    {
        id: '4',
        nome: 'Carlos Eduardo Santos',
        datadenascimento: "30/07/1990",
        email: 'carlos.santos@empresa.com',
        telefone: 31988776655,
        endereco: "Av. Afonso Pena, 2000 - Belo Horizonte/MG",
        cargo: "Analista de Dados",
        equipe: null,
        tarefaspostadas: 10,
        tarefasatrasadas:2,
        tarefasnaoentregues: 2,
        senha: 'carlos123',
        imagem: require('../../../../assets/img/fotoexemplo.png'),
    },
  ]);

  const equipeOrdenada = [...funcionario].sort((a, b) => {
    const scoreA = a.tarefaspostadas - (a.tarefasnaoentregues * 2 + a.tarefasatrasadas);
    const scoreB = b.tarefaspostadas - (b.tarefasnaoentregues * 2 + b.tarefasatrasadas);
    return scoreB - scoreA;
  }); //Por enquanto manter assim, depois mudar para se tornar mais dinamico.

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
        <View style={styles.nav}>
          <TouchableOpacity 
            style={styles.botaodevoltar}
            onPress={() => navigation.goBack()}
          >
            <Ionicons name="arrow-back" size={24} color={theme.text} />
          </TouchableOpacity>
            <Text style={styles.titulo}>WORKFLOW</Text>
            <View style={styles.espacoHeader} />
        </View>

        <View style={styles.containertarefas}>
          <Image source={equipe.imagem} style={styles.imag} />
          <View style={styles.textos}>
            <Text style={styles.textolistatitulo}>{equipe.titulo}</Text>
            <Text style={styles.textolistacargo}>{equipe.cargo}</Text>
          </View>
        </View>

        <Text style={styles.titulo2}>Ranking dos membros da equipe</Text>
          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisar funcionário"
            placeholderTextColor="#ffffff"
          />

        {funcionario.map((item, index)=> (
          <View 
            key={item.id} 
            style={styles.containertarefas}
          >   
            <Text style={styles.colocacao}>{index + 1}º</Text>
            <Image source={item.imagem} style={styles.imag} />
            <View style={styles.textos}>
              <Text style={styles.textolistatitulo}>{item.nome}</Text>
              <Text style={styles.textolistacargo}>{item.cargo}</Text>
            </View>
          </View>
        ))}
        </ScrollView>
      </View>
  );
}