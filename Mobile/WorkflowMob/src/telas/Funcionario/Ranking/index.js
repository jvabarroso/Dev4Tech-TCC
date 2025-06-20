import React, { useState } from 'react';
import { Text, View, Image, ScrollView, TextInput } from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'

export default function Ranking({navigation}){
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
      tarefasatrasadas:2,
      tarefasnaoentregues: 2,
      imagem: require('../../../../assets/img/image.png'),
    },
    {
      id: '3',
      titulo: 'Equipe 3',
      cargo: 'Desenvolvimento web',
      tarefaspostadas: 30,
      tarefasatrasadas:9,
      tarefasnaoentregues: 10,
      imagem: require('../../../../assets/img/image.png'),
    },
        {
      id: '4',
      titulo: 'Equipe 4',
      cargo: 'Analista de dados',
      tarefaspostadas: 30,
      tarefasatrasadas:9,
      tarefasnaoentregues: 10,
      imagem: require('../../../../assets/img/image.png'),
    },
  ]);
  const equipeOrdenada = [...equipe].sort((a, b, c, d) => {
    const scoreA = a.tarefaspostadas - (a.tarefasnaoentregues * 2 + a.tarefasatrasadas);
    const scoreB = b.tarefaspostadas - (b.tarefasnaoentregues * 2 + b.tarefasatrasadas);
    return scoreB - scoreA;
  }); //Por enquanto manter assim, depois mudar para se tornar mais dinamico.
    return(
      <View style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollContent}>
              <View>
                <Text style={styles.titulo}>Ranking de Equipes</Text>

                <TextInput
                  style={styles.navinput}
                  placeholder="🔍 Pesquisa uma equipe"
                  placeholderTextColor="#ffffff"
                />
              </View>

            {equipe.map((item, index) => (
              <View style={styles.containertarefas}>
                <Text style={styles.colocacao}>{index + 1}º</Text>
                <Image source={item.imagem} style={styles.imag} />
                <View style={styles.textos}>
                  <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                  <Text style={styles.textolistacargo}>{item.cargo}</Text>
                </View>
              </View>
            ))}
          </ScrollView>
        </View>
  );
}