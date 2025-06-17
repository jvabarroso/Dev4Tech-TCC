import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { styles } from './style';

import { Ionicons } from '@expo/vector-icons';

export default function CadastroTarefas({navigation}){
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
                <Text style={styles.titulo}>Adicionar uma tarefa</Text>
                <View style={styles.areaInput}>
                    <Text style={styles.texto}>Instruções</Text>
                    <TextInput
                        style={styles.inputinstrucoes}
                        multiline
                        numberOfLines={7}
                        placeholder="Alteração nos valores contratuais."
                        placeholderTextColor={"#000000"}
                    />
                    <TouchableOpacity
                        style={[styles.botaoanexo,styles.linha]}
                    >
                        <Ionicons name="document-text-outline" size={18} color="#3288D7" />
                        <Text style={styles.textoanexo}>Anexar Arquivo</Text>
                    </TouchableOpacity>

                    <Text style={styles.texto}>Equipes</Text>
                        <TouchableOpacity
                            style={[styles.input, styles.linha]}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{equipeselecionada || "Selecione uma equipe"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
                        </TouchableOpacity>
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
                    <Text style={styles.texto}>Data e entrega</Text>
                    <TextInput
                        style={styles.input}
                        placeholder="xx/xx/xxxx"
                        placeholderTextColor={"#000000"}
                        secureTextEntry={true}
                    />
                    <TouchableOpacity style={styles.botaocriar}>
                        <Text style={styles.textocriar}>Criar</Text>
                    </TouchableOpacity>
                </View>
            </ScrollView>
        </View>
    )
}
