import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { styles } from './style';

import { Ionicons } from '@expo/vector-icons';

export default function CadastroEquipes({navigation}){
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

    const [equipes, setequipes] = useState(true);
    const [equipeselecionada, setEquipeselecionada] = useState(null);

    const cliqueinformacao = () => 
        {
            setequipes(valorAtual => !valorAtual); 
        };
    
    const confirmodeequipe = (equipeSelecionada) => 
        {   
            setEquipeselecionada(equipeSelecionada.titulo);
            setequipes(valorAtual => !valorAtual); 
        };

    
    return(
        <View style={styles.container}>
            <ScrollView contentContainerStyle={styles.scrollContent}>
                <Text style={styles.titulo}>Criar uma equipe</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Nome da Equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite o nome da equipe"
                        placeholderTextColor={"#000000"}
                    />
                    <Text style={styles.texto}>Categoria da equipe</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="Digite a categoria da equipe"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <Text style={styles.texto}>Adicionar membros à equipe</Text>
                    <View style={styles.linha}>
                        <TouchableOpacity
                            style={[styles.inputequipe, styles.linha]}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{equipeselecionada || "Selecione uma equipe"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
                        </TouchableOpacity>

                        <TouchableOpacity
                            style={[styles.inputadicionar, styles.linha]}
                        >
                            <Text style={styles.textobotao}>+</Text> 
                        </TouchableOpacity>
                    </View>

                    {!equipes && equipe.map(item => (
                        <TouchableOpacity
                            key={item.id}
                            style={styles.containerequipes}
                            onPress={() => confirmodeequipe(item)}
                        >
                        <Image source={item.imagem} style={styles.imag} />
                        <View style={styles.textos}>
                            <Text style={styles.textolistatitulo}>{item.titulo}</Text>
                            <Text style={styles.textolistacargo}>{item.cargo}</Text>
                        </View>
                        </TouchableOpacity>
                    ))}
                    <TouchableOpacity style={styles.botaocriar}>
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
