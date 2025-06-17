import React, { useState } from 'react';
import { Text, View, Image, FlatList, TextInput, TouchableOpacity,} from 'react-native';
import { Card, Title, Paragraph } from 'react-native-paper';
import { styles } from './style';

export default function Equipes({ navigation }) {
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
    }
  ]);
  const [equipeSelecionada, setEquipeSelecionada] = useState(null)

  const clique = (id) => {
    setEquipeSelecionada(id === equipeSelecionada ? null : id);
  };

  return (
    <View style={styles.container}>
      <FlatList
        data={equipe}
        keyExtractor={(item) => item.id}
        style={styles.flat}
        ListHeaderComponent={() => (
          <View>
            <Text style={styles.titulo}>Equipes</Text>

            <TextInput
              style={styles.navinput}
              placeholder="🔍 Pesquisa uma equipe"
              placeholderTextColor="#ffffff"
            />
          </View>
        )}
        
        renderItem={({ item }) => (
        <View>
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
                <TouchableOpacity onPress={() => navigation.navigate('Equipes')}>
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

                <TouchableOpacity onPress={() => navigation.navigate('Tarefas')}>
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

                <TouchableOpacity onPress={() => navigation.navigate('Ranking')}>
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
        )}
        contentContainerStyle={{ paddingBottom: 100 }}
      />
    </View>
  );
}