import React, { useState } from 'react';
import { Text, View, Image, FlatList, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { BarChart, Grid, XAxis} from 'react-native-svg-charts';
import { AnimatedCircularProgress } from 'react-native-circular-progress';

import { styles } from './style';
import { Ionicons } from '@expo/vector-icons';

export default function RankingEstastistico({navigation, route}){
    const { equipe, index } = route.params;
    const [verificacaoinfor, setVerificacaoinfor] = useState(true);
    const [verificacaodesem, setVerificacaodesem] = useState(true);
    const [verificacaoentre, setVerificacaoentre] = useState(true);

    const cliqueinformacao = () => {setVerificacaoinfor(valorAtual => !valorAtual);};
    const cliquedesempenho = () => {setVerificacaodesem(valorAtual => !valorAtual);};
    const cliqueentrega = () => {setVerificacaoentre(valorAtual => !valorAtual);};


    const data = [
    { value: equipe.tarefaspostadas, svg: { fill: '#8000FF', rx: 10, ry: 10 } },
    { value: equipe.tarefasatrasadas, svg: { fill: '#3288D7', rx: 10, ry: 10 } },
    { value: equipe.tarefasnaoentregues, svg: { fill: '#FF0F00', rx: 10, ry: 10 } },
    ];
    const labels = ['Tarefas postadas', 'Tarefas Atrasadas', 'Tarefas não entregues'];
    const labelsdata = [equipe.tarefaspostadas, equipe.tarefasatrasadas, equipe.tarefasnaoentregues];
    

    const pontosganhos = equipe.tarefaspostadas;
    const pontosperdidos = equipe.tarefasnaoentregues * 2 + equipe.tarefasatrasadas
    const total = pontosganhos + pontosperdidos
    const desempenho = total > 0
    ? (pontosganhos/total)*100
    : 0;

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

                <Text style={styles.titulossub}>Raking de Equipe</Text>
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

                    <View style={[styles.linha, { alignItems: 'center', justifyContent: 'space-between' }]}>
                        <Text style={styles.titulodetalhes}>Contribuições</Text>
                        <TouchableOpacity
                            onPress={cliqueinformacao}>
                             <Ionicons name="information-circle-outline" size={22} color="#00000" style={{paddingHorizontal:5, marginRight:270, paddingVertical:5}}/>
                        </TouchableOpacity>
                    </View>   

                    {!verificacaoinfor && (
                        <View style={styles.containerbarras}>
                             <View style={styles.colunagrafico}>
                                {labels.map((label, index) => (
                                <Text key={index} style={styles.textobarras}>{label}</Text>
                                ))}
                            </View>
                            <View style={styles.colunagrafico}>
                                {labelsdata.map((label, index) => (
                                <Text key={index} style={[styles.textobarras, styles.color]}>{label}</Text>
                                ))}
                            </View>
                            <BarChart
                                style={styles.barras}
                                data={data}
                                horizontal={true}
                                yAccessor={({ item }) => item.value}
                                contentInset={{ top: 4, bottom: 8 }}
                                spacingInner={0.6}
                                gridMin={0}
                            >
                            </BarChart>
                        </View>
                    )}

                    <View style={[styles.linha, { alignItems: 'center', justifyContent: 'space-between' }]}>
                        <Text style={styles.titulodetalhes}>Desempenho</Text>
                        <TouchableOpacity
                            onPress={cliquedesempenho}>
                             <Ionicons name="information-circle-outline" size={22} color="#00000" style={{paddingHorizontal:5, marginRight:270, paddingVertical:5}}/>
                        </TouchableOpacity>
                    </View>   

                    {!verificacaodesem && (
                        <View style={styles.circleProgressView}>
                            <AnimatedCircularProgress
                                size={115}
                                width={25}
                                fill={desempenho}
                                tintColor="#1C58F2"
                                backgroundColor="#e0e0e0"
                                lineCap={"round"}
                            >
                            </AnimatedCircularProgress>
                            <View style={styles.areapontos}>
                                <View style={styles.linhaIconeTexto}>
                                    <View style={styles.azul}></View>
                                    <Text style={styles.textopontos}>Pontos ganhos   {pontosganhos}</Text>
                                </View>
                                <View style={styles.linhaIconeTexto}>
                                    <View style={styles.cinza}></View>
                                    <Text style={styles.textopontos}>Pontos Perdidos  {pontosperdidos}</Text>
                                </View>
                            </View>   
                        </View>
                    )}  

                    <View style={[styles.linha, { alignItems: 'center', justifyContent: 'space-between' }]}>
                        <Text style={styles.titulodetalhes}>Entrega de Tarefas</Text>
                        <TouchableOpacity
                            onPress={cliqueentrega}>
                             <Ionicons name="information-circle-outline" size={22} color="#00000" style={{paddingHorizontal:5, marginRight:270, paddingVertical:5}}/>
                        </TouchableOpacity>
                    </View>   

                    {!verificacaoentre && (
                        <View style={styles.circleProgressView}>
                            <AnimatedCircularProgress
                                size={115}
                                width={25}
                                fill={desempenho}
                                tintColor="#5BB14F"
                                backgroundColor="#e0e0e0"
                                lineCap={"round"}
                            >
                                {
                                    (fill) => (
                                        <Text style={styles.numberInside}>{`${Math.round(fill)}%`}</Text>
                                    )
                                }
                            </AnimatedCircularProgress>
                            <View style={styles.areapontos}>
                                <View style={styles.linhaIconeTexto}>
                                    <View style={styles.verde}></View>
                                    <Text style={styles.textopontos}>Tarefas entregues</Text>
                                </View>
                                <View style={styles.linhaIconeTexto}>
                                    <View style={styles.cinza}></View>
                                    <Text style={styles.textopontos}>Tarefas não entregues</Text>
                                </View>
                            </View>   
                        </View>
                    )}  
                </View> 
            </ScrollView> 
        </View>
  );
}