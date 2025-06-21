import React, { useState } from 'react';
import { Text, View, Image, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

export default function Equipes({ navigation }) {
  const { theme } = useTheme();
  const styles = getStyles(theme);
  
  const [equipe, setEquipe] = useState([
    {
      id: '1',
      titulo: 'Equipe 1',
      cargo: 'Desenvolvimento de Software',
      tarefaspostadas: 20,
      quantdeproblemas:6,
      tarefasatrasadas:1,
      tarefasnaoentregues: 6,
      imagem: require('../../../../assets/img/image.png'),
    },
    {
      id: '2',
      titulo: 'Equipe 2',
      cargo: 'Design',
      tarefaspostadas: 10,
      quantdeproblemas:2,
      tarefasatrasadas:2,
      tarefasnaoentregues: 2,
      imagem: require('../../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Equipe 3',
      cargo: 'Marketing Digital',
      tarefaspostadas: 15,
      quantdeproblemas: 3,
      tarefasatrasadas: 1,
      tarefasnaoentregues: 1,
      imagem: require('../../../../assets/img/image.png'),
    },
  ]);

  const [equipeSelecionada, setEquipeSelecionada] = useState(null)
  const [equipetela, setEquipetela] = useState(true)

  const [equipefuncionario, setEquipefuncionario] = useState(false)
  const [equipetarefas, setEquipetarefas] = useState(false)
  const [equiperanking, setEquiperanking] = useState(false)
  

  const clique = (id) => {
    setEquipeSelecionada(id === equipeSelecionada ? null : id);
  };

  return (
    <View style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollContent}>
          <Text style={styles.titulo}>Equipes</Text>
          <TextInput
            style={styles.navinput}
            placeholder="🔍 Pesquisa uma equipe"
            placeholderTextColor={"#ffffff"}
          />

          {equipe.map(item => (
          <View key={item.id}>
              <TouchableOpacity
                style={styles.containertarefas}
                onPress={() => clique(item.id)}
              >
                <Image source={item.imagem} style={styles.imag} />
                <View style={styles.textos}>
                  <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                  <Text style={styles.textolistacargo}>{item.cargo}</Text>
                </View>
              </TouchableOpacity>
                {equipeSelecionada === item.id && (
                <View style={styles.areacard}>
                  <TouchableOpacity onPress={() => navigation.navigate('EquipeFuncionario', { equipe: item })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/equipes.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Funcionarios</Title>
                        <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                        <View style={styles.linhainfer}>
                          <Text style={styles.data}>16/07/20</Text>
                          <Text style={styles.Entre}>Entre aqui</Text>
                        </View>
                      </Card.Content>
                    </Card>
                  </TouchableOpacity>

                  <TouchableOpacity onPress={() => navigation.navigate('EquipeTarefas', { equipe: item })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/tarefas.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Tarefas</Title>
                        <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                        <View style={styles.linhainfer}>
                          <Text style={styles.data}>16/07/20</Text>
                          <Text style={styles.Entre}>Entre aqui</Text>
                        </View>
                      </Card.Content>
                    </Card>

                  </TouchableOpacity>

                  <TouchableOpacity onPress={() => navigation.navigate('EquipeRanking', { equipe: item })}>

                    <Card style={styles.cardtarequi}>
                      <Card.Cover source={require('../../../../assets/img/ranking.png')} style={styles.imagemcard} />
                      <Card.Content style={styles.cardinferior}>
                        <Title style={styles.titulocard}>Ranking</Title>
                        <Paragraph style={styles.paragraph}>The point of using Lorem Ipsum is that....</Paragraph>
                        <View style={styles.linhainfer}>
                          <Text style={styles.data}>16/07/20</Text>
                          <Text style={styles.Entre}>Entre aqui</Text>
                        </View>
                      </Card.Content>
                    </Card>

                  </TouchableOpacity>
                </View>
              )}
            </View>
          ))}
      </ScrollView>
    </View>
  );
}