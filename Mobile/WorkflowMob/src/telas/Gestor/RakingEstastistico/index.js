import React, { useState } from 'react';
import { Text, View, Image, FlatList, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { BarChart, Grid, XAxis} from 'react-native-svg-charts';
import { AnimatedCircularProgress } from 'react-native-circular-progress';

import { styles } from './style';
import { Ionicons } from '@expo/vector-icons';

export default function RankingEstastistico({navigation, route}){
    const { equipe, index } = route.params;
    const [verificacao, setVerificacao] = useState(true);


    const data = [equipe.tarefaspostadas, equipe.tarefasatrasadas, equipe.tarefasnaoentregues];
    const labels = ['Tarefas postadas', 'Tarefas Atrasadas', 'Tarefas não entregues'];

    const cliqueinformacao = () => {
        setVerificacao(valorAtual => !valorAtual);
    };

    return(
      <View style={styles.container}>
            <ScrollView 
                style={styles.scrollView}
                contentContainerStyle={styles.containerConteudo}
                showsVerticalScrollIndicator={false}
            >
                <View style={styles.nav}>
                    <TouchableOpacity 
                        style={styles.botaodevoltar}
                        onPress={() => navigation.navigate('HomeAdm', { screen: 'RankingAdm' })}
                    >
                        <Ionicons name="arrow-back" size={24} color="black" />
                    </TouchableOpacity>
                    <Text style={styles.titulo}>WORKFLOW</Text>
                    <View style={styles.espacoHeader} />
                </View>

                <Text style={styles.titulossub}>Raking de Equipes</Text>
                <TextInput
                    style={styles.navinput}
                    placeholder="🔍 Pesquisa uma tarefa"
                    placeholderTextColor="#ffffff"
                />
                <View style={styles.containertarefas}>
                    <Text style={styles.colocacao}>{index + 1}º</Text>
                    <Image source={equipe.imagem} style={styles.imag} />
                    <View style={styles.textos}>
                        <Text style={styles.textolistatitulo}>{equipe.titulo}</Text>
                        <Text style={styles.textolistacargo}>{equipe.cargo}</Text>
                    </View>
                </View>

                <View style={styles.containerestatisticas}>
                    <View style={styles.linha}>
                        <Ionicons name="bar-chart-outline" size={40} color="#00000" style={{paddingHorizontal:5}}/>
                        <Text style={styles.tituloestastisca}>Estastísticas</Text>
                    </View>

                    <View style={styles.linha}>
                        <Text style={styles.titulodetalhes}>Contribuições</Text>
                        <TouchableOpacity
                            onPress={cliqueinformacao}>
                             <Ionicons name="information-circle-outline" size={22} color="#00000" style={{paddingHorizontal:2, marginRight:270, paddingVertical:5}}/>
                        </TouchableOpacity>
                    </View>                      
                    {!verificacao && (
                        <View style={styles.containerbarras}>
                             <View style={styles.colunagrafico}>
                                {labels.map((label, index) => (
                                <Text key={index} style={styles.textobarras}>{label}</Text>
                                ))}
                            </View>
                            <BarChart
                                style={styles.barras}
                                data={data}
                                horizontal={true}
                                svg={{ fill: '#1C58F2', rx: 20 }}
                                contentInset={{ top: 10, bottom: 10 }}
                                spacingInner={0.4}
                                gridMin={0}
                            >
                            </BarChart>
                        </View>
                        )}

                </View>
            </ScrollView> 
        </View>
  );
}