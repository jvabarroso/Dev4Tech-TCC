import React, { useState, useEffect } from 'react';
import { Text, View, Image, FlatList, TextInput, TouchableOpacity, ScrollView} from 'react-native';
import { BarChart, Grid, XAxis} from 'react-native-svg-charts';
import { AnimatedCircularProgress } from 'react-native-circular-progress';

import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'
import { Ionicons } from '@expo/vector-icons';
import api from '../../../../services/api';

export default function RankingEstastistico({navigation, route}){
    const { theme } = useTheme();
    const styles = getStyles(theme);

    const equipe = route.params?.equipe || {}; 
    const posicaoOriginal = route.params?.posicaoOriginal || {}; 

    const [dados, setDados] = useState([]);
    const maxPontos = Math.max(...dados.map(item => item.pontos), 1);

    const [verificacaoinfor, setVerificacaoinfor] = useState(true);
    const [verificacaodesem, setVerificacaodesem] = useState(true);
    const [verificacaoentre, setVerificacaoentre] = useState(true);

    const cliqueinformacao = () => {setVerificacaoinfor(valorAtual => !valorAtual);};
    const cliquedesempenho = () => {setVerificacaodesem(valorAtual => !valorAtual);};
    const cliqueentrega = () => {setVerificacaoentre(valorAtual => !valorAtual);};
    
    //Lista Pontos da equipe
    async function listarpontos() {
        try {
        const res = await api.get(`dev4tech/pontuacaofuncionariografico.php`, {
        params: {id_equipe: equipe.id_equipe }
        });

        if (res.data.success) {
        setDados(res.data.result || []);
        } else {
            console.log("Erro na API:", res.data.message);
            setDados([]);
        }
        }
        catch (error) {
        console.log("Erro ao listar funcionarios", error);
        }
    }

    useEffect(() => {
        listarpontos();
    }, [equipe?.id_equipe]);


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
                        onPress={() => navigation.goBack()}
                    >
                        <Ionicons name="arrow-back" size={24} color={theme.text} />
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
                    <Text style={styles.colocacao}>{posicaoOriginal}º</Text>
                    <Image 
                      source={equipe.foto_equipe ? { uri: equipe.foto_equipe } : require('../../../../assets/img/image.png')} 
                      style={styles.imag} 
                    />
                    <View style={styles.textos}>
                      <Text style={styles.textolistatitulo}>{equipe.nome_equipe}</Text>
                      <Text style={styles.textolistacargo}>{equipe.nome_categoria}</Text>
                    </View>
                </View>

                <View style={styles.containerestatisticas}>
                    <View style={styles.linha}>
                        <Ionicons name="bar-chart-outline" size={40} color="#00000" style={{paddingHorizontal:5}}/>
                        <Text style={styles.tituloestastisca}>Estastísticas</Text>
                    </View>

                    <View style={[styles.linha, { alignItems: 'center', justifyContent: 'space-between' }]}>
                        <Text style={styles.titulodetalhes}>Contribuições de funcionarios</Text>
                        <TouchableOpacity
                            onPress={cliqueinformacao}>
                             <Ionicons name="information-circle-outline" size={22} color="#00000" style={{paddingHorizontal:5, marginRight:270, paddingVertical:5}}/>
                        </TouchableOpacity>
                    </View>   

                    {!verificacaoinfor && (
                        <View style={styles.containerbarras}>
                           {dados.map((item, index) => ( 
                            <View key={index} style={styles.areafuncionario}>

                                <Text style={[styles.textobarras, { width:90 }]} numberOfLines={2} ellipsizeMode="tail">
                                    {item.nome}
                                </Text>

                                <Text style={[styles.textobarras, styles.color, { width: 50, textAlign: 'right', marginRight: 10 }]}>
                                    {item.pontos}
                                </Text>
                                
                                <View style={styles.barras}>
                                    <View style={[styles.barra, {width: `${(item.pontos / maxPontos) * 100}%` }]}
                                />
                                </View>
                            </View>
                            ))}
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