import React, { useState } from 'react';
import { Text, View, TouchableOpacity, Image, ScrollView, TextInput} from 'react-native';
import { getStyles } from './style';
import { useTheme } from '../../../styles/themecontext'


import { Ionicons } from '@expo/vector-icons';

export default function CadastroFuncionario({navigation}){
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
            <Text style={styles.titulo}>Cadastrar Funcionário</Text>
                    <View style={styles.areaInput}>
                        <Text style={styles.texto}>Nome do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="Gabriel Kenzo" //depois mudar, mensagem para mim mesmo dnv :D
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Data de nascimento</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="xx/xx/xxxx"
                            placeholderTextColor={theme.text}
                            secureTextEntry={true}
                        />
                        <Text style={styles.texto}>Email</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="joaovitinhocraft@gmail.com"
                            placeholderTextColor={theme.text}
                            secureTextEntry={true}
                        />
                        <Text style={styles.texto}>Telefone</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="1399899989"
                            placeholderTextColor={theme.text}
                        />
                        <Text style={styles.texto}>Endereço</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="Rua João da Fonseca, Jardim Mato Grosso, Cananeia senha"
                            placeholderTextColor={theme.text}
                            secureTextEntry={true}
                        />
                        <Text style={styles.texto}>Categoria do funcionário</Text>
                        <TextInput
                            style={styles.input}
                            placeholder="Analista de Marketing"
                            placeholderTextColor={theme.text}
                            secureTextEntry={true}
                        />
                        <Text style={styles.texto}>Adicionar a uma equipe</Text>
                        <TouchableOpacity
                            style={[styles.input, styles.linha]}
                            onPress={cliqueinformacao}
                        >
                            <Text style={styles.textobotao}>{equipeselecionada || "Selecione uma equipe"}</Text> 
                            <Ionicons name="caret-down-outline" size={18} color="black" />
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
            </ScrollView>
        </View>
    );
}